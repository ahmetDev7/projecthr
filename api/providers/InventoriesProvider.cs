using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class InventoriesProvider : BaseProvider<Inventory>
{
    private readonly ItemsProvider _itemsProvider;
    private IValidator<Inventory> _inventoryValidator;
    private IValidator<InventoryRequest> _inventoryRequestValidator;


    public InventoriesProvider(AppDbContext db, ItemsProvider itemsProvider, IValidator<Inventory> inventoryValidator, IValidator<InventoryRequest> inventoryRequestValidator) : base(db)
    {
        _itemsProvider = itemsProvider;
        _inventoryValidator = inventoryValidator;
        _inventoryRequestValidator = inventoryRequestValidator;
    }

    public override Inventory? GetById(Guid id) => _db.Inventories.Include(i => i.Item).FirstOrDefault(i => i.Id == id);

    public override List<Inventory>? GetAll() => _db.Inventories.Include(i => i.Item).ToList();

    public override Inventory? Create(BaseDTO createValues)
    {
        InventoryRequest? req = createValues as InventoryRequest;
        if (req == null) throw new ApiFlowException("Invalid inventory request. Could not create inventory.");

        _inventoryRequestValidator.ValidateAndThrow(req);

        Inventory newInventory = new(newInstance: true)
        {
            Id = Guid.NewGuid(),
            Description = req.Description,
            ItemReference = req.ItemReference,
            ItemId = req.ItemId,
        };

        ValidateModel(newInventory);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            _db.Inventories.Add(newInventory);
            FillInventoryLocations(req.Locations, newInventory.Id);
            newInventory.TotalOnHand = CalculateTotalOnHand(newInventory.Id);
            SaveToDBOrFail();

            setCaluclatedValues(newInventory);
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

        return newInventory;
    }

    public override Inventory? Update(Guid id, BaseDTO updatedValues)
    {
        InventoryRequest? req = updatedValues as InventoryRequest;
        if (req == null) throw new ApiFlowException("Invalid inventory request. Could not update inventory.");

        _inventoryRequestValidator.ValidateAndThrow(req);

        Inventory? foundInventory = GetById(id);
        if (foundInventory == null) return null;

        // Transaction src: https://learn.microsoft.com/en-us/ef/core/saving/transactions
        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            if (req.ItemId.HasValue)
            {
                Item? foundItem = _itemsProvider.GetById(id: req.ItemId.Value, includeInventory: true);
                if (foundItem == null) throw new ApiFlowException($"Item not found for id '{req.ItemId.Value}'", StatusCodes.Status404NotFound);

                if (foundInventory.ItemId.Value != foundItem.Id && foundItem.Inventory != null)
                    throw new ApiFlowException($"This item for id '{foundItem.Id}' already has an allocated inventory please select a different item.", StatusCodes.Status409Conflict);

                foundInventory.ItemId = foundItem.Id;
            }

            foundInventory.Description = req.Description;
            foundInventory.ItemReference = req.ItemReference;

            // Unlink inventory locations with ExecuteDelete: https://learn.microsoft.com/en-us/ef/core/saving/execute-insert-update-delete#executeupdate
            _db.InventoryLocations.Where(il => il.InventoryId == foundInventory.Id).ExecuteDelete();

            FillInventoryLocations(req.Locations, foundInventory.Id);
            foundInventory.SetUpdatedAt();
            ValidateModel(foundInventory);
            SaveToDBOrFail();

            setCaluclatedValues(inventory: foundInventory);
            transaction.Commit();

            return foundInventory;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
            /*
                NOTE: 
                    - Intentional throw so that GlobalExceptionMiddleware will handle the response     
                    - https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2200#example  
            */
        }
    }

    public override Inventory? Delete(Guid id)
    {
        Inventory? foundInventory = GetById(id);
        if (foundInventory == null) return null;

        _db.Inventories.Remove(foundInventory);
        SaveToDBOrFail();
        return foundInventory;
    }

    protected override void ValidateModel(Inventory model) => _inventoryValidator.ValidateAndThrow(model);

    private void FillInventoryLocations(List<InventoryLocationRR> inventoryLocations, Guid inventoryId)
    {
        foreach (InventoryLocationRR location in inventoryLocations)
        {
            _db.InventoryLocations.Add(new InventoryLocation(newInstance: true)
            {
                InventoryId = inventoryId,
                LocationId = location.LocationId,
                OnHandAmount = location.OnHand ?? 0
            });
        }
    }

    public Inventory? GetInventoryByItemId(Guid? itemId) => _db.Inventories.Where(i => i.ItemId == itemId).FirstOrDefault();

    public List<InventoryLocation>? GetInventoryLocations(Guid inventoryId) => _db.InventoryLocations
        .Include(il => il.Location)
        .Where(il => il.InventoryId == inventoryId)
        .ToList();

    public void setCaluclatedValues(Inventory? inventory = null, Guid? inventoryId = null)
    {
        if (inventoryId.HasValue) inventory = GetById(inventoryId.Value);

        if (inventory == null) throw new Exception("Processing inventory calculated values failed.");

        inventory.TotalOnHand = CalculateTotalOnHand(inventory.Id);
        inventory.TotalAllocated = 0; //TODO:
        inventory.TotalAvailable = 0; //TODO:
        inventory.TotalOrderd = 0; //TODO:
        inventory.TotalExpected = 0; //TODO:
        inventory.SetUpdatedAt();
        _db.Inventories.Update(inventory);
        SaveToDBOrFail();
    }

    /*
        total_on_hand int FIXME: Optellen op basis van inventory_locations SUM
        total_expected int FIXME: (inbound shipments) items amount optellen
        total_ordered int FIXME: als het ordered is maar nog niet klaar is (alles optellen met status pending)
        total_allocated int FIXME: als order is afgerond en shipment is onderweg (toegewezen aan shipment)
        total_available int FIXME: (total on hand + total expected) - (total ordered + total allocated)
    */

    public void CalculateTotalExpected(Guid? itemId)
    {
        int totalExpected = _db.ShipmentItems.Include(si => si.Shipment).Where(si => si.ItemId == itemId && si.Shipment.ShipmentType == ShipmentType.I).Sum(si => si.Amount) ?? 0;
        Inventory? inventory = GetInventoryByItemId(itemId);

        if (inventory == null)
        {
            throw new ApiFlowException($"An error occurred while updating the total expected quantity for item with ID: {itemId}. Please ensure the item exists in inventory.");
        }

        inventory.TotalExpected = totalExpected;
        _db.Inventories.Update(inventory);
        SaveToDBOrFail();
    }

    public void CalculateTotalOrderd(Guid? itemId)
    {
        int totalOrderd = _db.OrderItems.Include(o => o.Order).Where(o => o.ItemId == itemId && o.Order.OrderStatus == OrderStatus.Pending).Sum(o => o.Amount) ?? 0;
        Inventory? inventory = GetInventoryByItemId(itemId);
        if (inventory == null)
        {
            throw new ApiFlowException($"An error occurred while updating the total orderd quantity for item with ID: {itemId}. Please ensure the item exists in inventory.");
        }

        inventory.TotalOrderd = totalOrderd;
        _db.Inventories.Update(inventory);
        SaveToDBOrFail();
    }

    public void CalculateTotalAllocated(Guid itemId, Guid? shipmentIdThatWasInTransit = null)
    {
        List<Order>? closedOrders = _db.Orders
            .Where(o => o.OrderStatus == OrderStatus.Closed)
            .Include(o => o.OrderShipments)
            .ThenInclude(os => os.Shipment)
            .ThenInclude(s => s.ShipmentItems)
            .ToList();

        if (closedOrders == null || !closedOrders.Any())
        {
            return;
        }

        int? totalAllocated = 0;
        foreach (Order? closedOrder in closedOrders)
        {
            foreach (OrderShipment? orderShipment in closedOrder.OrderShipments ?? [])
            {

                // Check either shipmentIdThatWasInTransit or shipment status
                if (shipmentIdThatWasInTransit.HasValue)
                {
                    if (orderShipment.ShipmentId != shipmentIdThatWasInTransit) continue;
                }
                else
                {
                    if (orderShipment.Shipment?.ShipmentStatus != ShipmentStatus.Transit) continue;
                }

                totalAllocated += orderShipment?.Shipment?.ShipmentItems?.Where(item => item.ItemId == itemId).Sum(item => item.Amount);
            }
        }
        Inventory? inventory = GetInventoryByItemId(itemId);
        if (inventory == null)
        {
            throw new ApiFlowException($"An error occurred while updating the total orderd quantity for item with ID: {itemId}. Please ensure the item exists in inventory.");
        }

        inventory.TotalAllocated = totalAllocated ?? 0;
        _db.Inventories.Update(inventory);
        SaveToDBOrFail();
    }


    public int CalculateTotalOnHand(Guid inventoryId) => _db.InventoryLocations.Where(il => il.InventoryId == inventoryId).Sum(il => il.OnHandAmount);
}