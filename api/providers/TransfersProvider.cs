using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class TransferProvider : BaseProvider<Transfer>
{
    private IValidator<Transfer> _transferValidator;
    private readonly LocationsProvider _locationsProvider;
    private readonly InventoriesProvider _inventoriesProvider;

    public TransferProvider(AppDbContext db, IValidator<Transfer> validator, LocationsProvider locationsProvider, InventoriesProvider inventoriesProvider) : base(db)
    {
        _transferValidator = validator;
        _locationsProvider = locationsProvider;
        _inventoriesProvider = inventoriesProvider;
    }

    public override Transfer? GetById(Guid id)
    {
        return _db.Transfers.Include(t => t.TransferItems).FirstOrDefault(t => t.Id == id);
    }

    public override List<Transfer>? GetAll() => _db.Transfers.Include(t => t.TransferItems).ToList();

    public override Transfer? Create(BaseDTO createValues)
    {
        TransferRequestCreate? req = createValues as TransferRequestCreate ?? throw new ApiFlowException("Could not process create transfer request. Save new transfer failed.");

        Transfer newTransfer = new(newInstance: true)
        {
            TransferFromId = req.TransferFromId,
            TransferToId = req.TransferToId,
            Reference = req.Reference,
            TransferItems = req.Items?.Select(reqItem => new TransferItem(newInstance: true)
            {
                Id = Guid.NewGuid(),
                ItemId = reqItem.ItemId,
                Amount = reqItem.Amount
            }).ToList()
        };

        newTransfer.TransferStatus = TransferStatus.Pending;
        ValidateModel(newTransfer);

        _db.Transfers.Add(newTransfer);
        SaveToDBOrFail();
        return newTransfer;
    }

    public Transfer? CommitTransfer(Guid transferId)
    {
        Transfer? foundTransfer = GetById(transferId) ?? throw new ApiFlowException($"Transfer with ID {transferId} does not exist.", StatusCodes.Status404NotFound);
        if(foundTransfer.TransferStatus == TransferStatus.Completed)
            throw new ApiFlowException("This transfer has already been commited.", StatusCodes.Status409Conflict);
        
        ValidateModel(foundTransfer);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            bool isFromLocation = foundTransfer.TransferFromId.HasValue;
            bool isToLocation = foundTransfer.TransferToId.HasValue;

            bool isFromDock = !isFromLocation;
            bool isToDock = !isToLocation;

            Location? fromLocation = null;
            Location? toLocation = null;
            Dock? dock = null;
            DockItem? dockItem = null;

            TransferItem? firstItem = foundTransfer.TransferItems.First();
            Inventory? ItemInventory = _inventoriesProvider.GetInventoryByItemId(itemId: firstItem.ItemId.Value); // because all items are the same always take the first one


            if (isFromLocation)
            {
                fromLocation = _locationsProvider.GetById(foundTransfer.TransferFromId.Value, includeWarehouse: true, includeInventory: true);
                dock = fromLocation.Warehouse.Dock;
            }
            if (isToLocation)
            {
                toLocation = _locationsProvider.GetById(foundTransfer.TransferToId.Value, includeWarehouse: true, includeInventory: true);
                if (dock == null) dock = toLocation.Warehouse.Dock;
            }

            if (isFromDock || isToDock)
            {
                dockItem = _db.DockItems.FirstOrDefault(di => di.DockId == dock.Id && di.ItemId == firstItem.ItemId);
            }

            int? totalAmount = foundTransfer.TransferItems.Sum(i => i.Amount);

            if (isFromLocation)
            {
                fromLocation.OnHand -= totalAmount;
            }

            if (isToLocation)
            {
                // if inventory_id is empty set inventory_id with on_hand
                if (!toLocation.InventoryId.HasValue) toLocation.InventoryId = ItemInventory.Id;

                // if inventory_id is the same inventory_id as item then increment on_hand amount
                toLocation.OnHand = (toLocation.OnHand ?? 0) + totalAmount;
            }

            if (isToDock)
            {
                // create or increment the item that is going to be at DockItem
                if (dockItem == null)
                {
                    DockItem newDockItem = new(newInstance: true)
                    {
                        ItemId = firstItem.ItemId,
                        DockId = dock.Id,
                        Amount = totalAmount
                    };
                    _db.DockItems.Add(newDockItem);
                }
                else
                {
                    dockItem.Amount += totalAmount;
                    dockItem.SetUpdatedAt();
                }
            }

            // remove or decrement the item that is going to be removed from dock
            if (isFromDock)
            {
                dockItem.Amount -= totalAmount;
                if (dockItem.Amount == null)
                    dockItem.SetUpdatedAt();
                _db.DockItems.Remove(dockItem);
            }

            foundTransfer.TransferStatus = TransferStatus.Completed;
            SaveToDBOrFail();
            transaction.Commit();


            return foundTransfer;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    protected override void ValidateModel(Transfer model) => _transferValidator.ValidateAndThrow(model);
}