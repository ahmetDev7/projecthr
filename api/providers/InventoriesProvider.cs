using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class InventoriesProvider : BaseProvider<Inventory>
{
    private readonly LocationsProvider _locationsProvider;
    private readonly ItemsProvider _itemsProvider;
    private IValidator<Inventory> _inventoryValidator;
    private IValidator<InventoryRequest> _inventoryRequestValidator;


    public InventoriesProvider(AppDbContext db, LocationsProvider locationsProvider, ItemsProvider itemsProvider, IValidator<Inventory> inventoryValidator, IValidator<InventoryRequest> inventoryRequestValidator) : base(db)
    {
        _locationsProvider = locationsProvider;
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
            Description = req.Description,
            ItemReference = req.ItemReference,
            ItemId = req.ItemId,
        };

        ValidateModel(newInventory);
        _db.Inventories.Add(newInventory);

        _locationsProvider.FillOnHandAmount(newInventory.Id, req.Locations, false);
        SaveToDBOrFail();

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
                if (foundItem == null) throw new ApiFlowException($"Item not found for id '{req.ItemId.Value}'");

                if (foundInventory.ItemId.Value != foundItem.Id && foundItem.Inventory != null)
                    throw new ApiFlowException($"This item for id '{foundItem.Id}' already has an allocated inventory please select a different item.");

                foundInventory.ItemId = foundItem.Id;
            }

            foundInventory.Description = req.Description;
            foundInventory.ItemReference = req.ItemReference;

            UnlinkInventoryLocations(foundInventory.Id);
  
            _locationsProvider.FillOnHandAmount(foundInventory.Id, req.Locations, false);
            foundInventory.SetUpdatedAt();

            ValidateModel(foundInventory);
            SaveToDBOrFail();
            transaction.Commit();
            
            return foundInventory;
        }
        catch(Exception)
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

    // Unlink inventory locations with ExecuteUpdate: https://learn.microsoft.com/en-us/ef/core/saving/execute-insert-update-delete#executeupdate
    private void UnlinkInventoryLocations(Guid id) =>
            _db.Locations.Where(l => l.InventoryId == id)
            .ExecuteUpdate(setters => setters
            .SetProperty(l => l.InventoryId, l => null)
            .SetProperty(l => l.OnHand, l => 0));

    public override Inventory? Delete(Guid id)
    {
        Inventory? foundInventory = GetById(id);
        if (foundInventory == null) return null;

        _db.Inventories.Remove(foundInventory);
        SaveToDBOrFail();
        return foundInventory;
    }

    public Inventory? GetInventoryByItemId(Guid? itemId) => _db.Inventories.Where(i => i.ItemId == itemId).FirstOrDefault();

    /*
        total_on_hand int //calculated value from location_inventory
        total_expected int //calculated value 
        total_ordered int //calculated value op basis van order
        total_allocated int //calculated value
        total_available int //calculated value
    */

    public int CalculateTotalOnHand(Guid inventoryId) => _db.Locations.Where(location => location.InventoryId == inventoryId).Sum(location => location.OnHand ?? 0);

    public int CalculateTotalExpected()
    {
        return 0; // TODO
    }

    public int CalculateTotalOrdered()
    {
        return 0; // TODO
    }

    public int CalculateTotalAllocated()
    {
        return 0; // TODO
    }

    public int CalculateTotalAvailable()
    {
        return 0; // TODO
    }

    public Dictionary<string, int> GetCalculatedValues(Guid inventoryId) => new Dictionary<string, int>()
        {
            {"TotalOnHand", CalculateTotalOnHand(inventoryId)},
            {"TotalExpected", CalculateTotalExpected()},
            {"TotalOrdered", CalculateTotalOrdered()},
            {"TotalAllocated", CalculateTotalAllocated()},
            {"TotalAvailable", CalculateTotalAvailable()},
        };



    protected override void ValidateModel(Inventory model) => _inventoryValidator.ValidateAndThrow(model);
}