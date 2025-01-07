using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class TransferProvider : BaseProvider<Transfer>
{
    private IValidator<Transfer> _transferValidator;
    private readonly LocationsProvider _locationsProvider;
    private readonly InventoriesProvider _inventoriesProvider;
    private readonly ItemsProvider _itemsProvider;

    public TransferProvider(AppDbContext db, IValidator<Transfer> validator, LocationsProvider locationsProvider, InventoriesProvider inventoriesProvider, ItemsProvider itemsProvider) : base(db)
    {
        _transferValidator = validator;
        _locationsProvider = locationsProvider;
        _inventoriesProvider = inventoriesProvider;
        _itemsProvider = itemsProvider;
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

    public override Transfer? Update(Guid id, BaseDTO updatedValues)
    {
        TransferRequestUpdate? req = updatedValues as TransferRequestUpdate;
        if (req == null) throw new ApiFlowException("Could not process update transfer request. Update failed.");

        Transfer? foundTransfer = GetById(id);
        if (foundTransfer == null) throw new ApiFlowException("Could not update transfer.", StatusCodes.Status404NotFound);

        foundTransfer.Reference = req.Reference;
        foundTransfer.TransferFromId = req.TransferFromId;
        foundTransfer.TransferToId = req.TransferToId;
        foundTransfer.TransferStatus = req.TransferStatus;
        _db.TransferItems.RemoveRange(foundTransfer.TransferItems);
        foundTransfer.TransferItems = req.Items?.Select(reqItem => new TransferItem(newInstance: true)
        {
            Id = Guid.NewGuid(),
            ItemId = reqItem.ItemId,
            Amount = reqItem.Amount
        }).ToList();

        ValidateModel(foundTransfer);
        SaveToDBOrFail();

        return foundTransfer;
    }

    public override Transfer? Delete(Guid id)
    {
        Transfer? foundTransfer = GetById(id);
        if (foundTransfer == null) return null;

        _db.Transfers.Remove(foundTransfer);
        SaveToDBOrFail();

        return foundTransfer;
    }

    public Transfer? CommitTransfer(Guid transferId)
    {
        Transfer? foundTransfer = GetById(transferId) ?? throw new ApiFlowException($"Transfer with ID {transferId} does not exist.", StatusCodes.Status404NotFound);

        if (foundTransfer.TransferStatus == TransferStatus.Completed)
            throw new ApiFlowException(message: "This transfer has already been commited.", StatusCodes.Status409Conflict);


        ValidateModel(foundTransfer);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            bool isFromLocation = foundTransfer.TransferFromId.HasValue;
            bool isToLocation = foundTransfer.TransferToId.HasValue;

            bool isFromDock = !isFromLocation;
            bool isToDock = !isToLocation;

            Location? fromLocation = isFromLocation
                ? _locationsProvider.GetById(foundTransfer.TransferFromId.Value, includeWarehouse: true)
                : null;

            Location? toLocation = isToLocation
                ? _locationsProvider.GetById(foundTransfer.TransferToId.Value, includeWarehouse: true)
                : null;

            Dock? dock = fromLocation?.Warehouse?.Dock ?? toLocation?.Warehouse?.Dock;

            List<DockItem>? dockItems = (isFromDock || isToDock)
                ? _db.DockItems.Where(di => di.DockId == dock.Id).ToList()
                : null;

            foreach (TransferItem transferItem in foundTransfer.TransferItems)
            {
                Guid? currentItemId = transferItem.ItemId;
                Inventory? currentItemInventory = _inventoriesProvider.GetInventoryByItemId(itemId: currentItemId);
                DockItem? itemInDock = (isFromDock || isToDock) ? _db.DockItems.FirstOrDefault(d => d.ItemId == currentItemId) : null;

                if (isFromLocation)
                {
                    InventoryLocation? inventoryLocation = _db.InventoryLocations.FirstOrDefault(il => il.InventoryId == currentItemInventory.Id && il.LocationId == foundTransfer.TransferFromId);
                    inventoryLocation.OnHandAmount -= transferItem.Amount.Value;
                }

                if (isToLocation)
                {
                    // the inventory location for tranfer_to_id
                    InventoryLocation? foundInventoryLocationTo = _db.InventoryLocations.FirstOrDefault(il => il.LocationId == foundTransfer.TransferToId);

                    // if the item does not exist on the transfer_to location then create a new inventoryLocation with the item
                    if (foundInventoryLocationTo == null)
                    {
                        _db.InventoryLocations.Add(new InventoryLocation(newInstance: true)
                        {
                            InventoryId = currentItemInventory.Id,
                            LocationId = foundTransfer.TransferToId,
                            OnHandAmount = transferItem.Amount.Value
                        });
                    }
                    else // if the item does exist on on the transfer_to location then increment the onHandAmount
                    {
                        foundInventoryLocationTo.OnHandAmount += transferItem.Amount.Value;
                        foundInventoryLocationTo.SetUpdatedAt();
                        _db.InventoryLocations.Update(foundInventoryLocationTo);
                    }

                }

                // create or increment the item that is going to be at DockItem
                if (isToDock)
                {
                    // if not found create dockitem
                    if (itemInDock == null)
                    {
                        _db.DockItems.Add(new DockItem(newInstance: true)
                        {
                            ItemId = currentItemId,
                            DockId = dock.Id,
                            Amount = transferItem.Amount
                        });
                    }
                    else
                    {
                        // if found increment
                        itemInDock.Amount += transferItem.Amount;
                        _db.DockItems.Update(itemInDock);
                    }
                }


                // remove or decrement the item that is going to be removed from dock
                if (isFromDock)
                {
                    itemInDock.Amount -= transferItem.Amount;

                    if (itemInDock.Amount == 0)
                    {
                        _db.DockItems.Remove(itemInDock);
                    }
                    else
                    {
                        itemInDock.SetUpdatedAt();
                        _db.DockItems.Update(itemInDock);
                    }
                }
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

    public List<Item> GetItemsFromTransfer(Transfer transfer)
    {
        List<Guid?> itemIds = transfer.TransferItems.Select(ti => ti.ItemId).ToList();
        return _db.Items.Where(i => itemIds.Contains(i.Id)).ToList();
    }

    public bool IsTransferCompleted(Guid transferId) => _db.Transfers.Any(t => t.Id == transferId && t.TransferStatus == TransferStatus.Completed);
    public bool TransferExists(Guid transferId) => _db.Transfers.Any(t => t.Id == transferId);

    protected override void ValidateModel(Transfer model) => _transferValidator.ValidateAndThrow(model);
}