using FluentValidation;

public class TransferValidator : AbstractValidator<Transfer>
{
    public TransferValidator(AppDbContext db, InventoriesProvider inventoriesProvider, LocationsProvider locationsProvider)
    {
        RuleFor(transfer => transfer).Custom((transfer, context) =>
        {

            if (transfer.TransferItems == null || transfer.TransferItems.Count == 0) return;

            // check if transfer_from and transfer_to are not the same
            if (transfer.TransferFromId == transfer.TransferToId)
            {
                context.AddFailure("transfer_from_id and transfer_to_id", "transfer_to_id and transfer_from_id cannot be the same.");
                return;
            }

            int? totalAmount = null;
            bool fromDock = transfer.TransferFromId == null;
            bool toDock = transfer.TransferToId == null;

            bool fromLocation = !fromDock;
            bool toLocation = !toDock;

            Warehouse? warehouseFrom = null;
            Warehouse? warehouseTo = null;

            if (fromLocation && transfer.TransferFromId.HasValue)
            {
                Location? locationFrom = locationsProvider.GetById(id: transfer.TransferFromId.Value, includeWarehouse: true);
                if (locationFrom == null) return;
                warehouseFrom = locationFrom.Warehouse;
                warehouseTo = locationFrom.Warehouse;
                if (warehouseFrom == null)
                {
                    context.AddFailure("transfer_from_id", $"Warehouse not found");
                    return;
                }
            }

            if (toLocation && transfer.TransferToId.HasValue)
            {
                Location? locationTo = locationsProvider.GetById(id: transfer.TransferToId.Value, includeWarehouse: true);
                if (locationTo == null) return;
                warehouseFrom = locationTo.Warehouse;
                warehouseTo = locationTo.Warehouse;

                if (warehouseTo == null)
                {
                    context.AddFailure("transfer_to_id", $"Warehouse not found");
                    return;
                }
            }

            if (warehouseFrom == null && warehouseTo == null)
            {
                context.AddFailure("transfer_from_id and transfer_to_id", $"Warehouse not dock not found for transfer_from_id and transfer_to_id");
                return;
            }

            if (fromLocation && toLocation && warehouseFrom.Id != warehouseTo.Id)
            {
                context.AddFailure("transfer_from_id and transfer_to_id", $"Both locations must be in the same warehouse");
                return;
            }

            if (fromDock || toDock)
            {
                totalAmount = transfer.TransferItems.Sum(i => i.Amount);
            }

            foreach (TransferItem transferItem in transfer.TransferItems)
            {
                Guid? itemId = transferItem.ItemId.Value;
                Inventory? foundInventory = null;

                // Checks if the item_id related to the inventory_id exists
                foundInventory = db.Inventories.FirstOrDefault(i => i.ItemId == itemId);
                if (foundInventory == null)
                {
                    context.AddFailure("items", $"The specified item (ID: {itemId}) has no inventory, and therefore, no allocated location exists for this item.");
                    return;
                }

                Guid currentInventoryId = foundInventory.Id;

                if (fromLocation)
                {
                    // check if the item.inventoryId is on the location (from_transfer_id)
                    InventoryLocation? locationOfTransferFrom = db.InventoryLocations.FirstOrDefault(il =>
                        il.InventoryId == currentInventoryId &&
                        il.LocationId == transfer.TransferFromId
                    );

                    // returns error if the given inventoryId is not found with the corresponding location of from_transfer_id
                    if (locationOfTransferFrom == null)
                    {
                        context.AddFailure("transfer_from", $"item_id {itemId} not found on the specified location of transfer_from_id {transfer.TransferFromId}");
                        return;
                    }

                    // check if the selected on_hand amount is lower or equal to the selected amount
                    int? transferAmount = transferItem.Amount;
                    if (locationOfTransferFrom.OnHandAmount < transferAmount)
                    {
                        context.AddFailure("items", "The transfer amount exceeds the available inventory at the source location. Please adjust the amount to match the on-hand quantity.");
                        return;
                    }
                }

                if (fromDock)
                {
                    // Find the item in the destination dock
                    DockItem? fromDockItem = db.DockItems.FirstOrDefault(di => di.DockId == warehouseFrom.Dock.Id && di.ItemId == itemId);

                    // Validate item existence in the dock
                    if (fromDockItem == null)
                    {
                        context.AddFailure("items", $"Item {itemId} not found in dock of warehouse {warehouseFrom.Id}");
                        return;
                    }

                    // Ensure the requested amount does not exceed available quantity
                    if (totalAmount > fromDockItem.Amount)
                    {
                        context.AddFailure(
                            "items",
                            $"The selected amount exceeds the available quantity on the dock. Current dock quantity: {fromDockItem.Amount}. Attempted quantity: {totalAmount}."
                        );
                        return;
                    }
                }

                if (toDock)
                {
                    // check if dock capacity is lower than 50 based on the total item amount in transfer and the fromDock location
                    int? totalOnDock = db.DockItems.Where(di => di.DockId == warehouseFrom.Dock.Id).Sum(di => di.Amount) ?? 0;
                    int? spaceAvailable = Dock.CAPCITY - totalOnDock;

                    if (totalAmount > spaceAvailable)
                    {
                        context.AddFailure("items", $"The selected amount exceeds the dock's capacity. The maximum capacity is {Dock.CAPCITY}, with only {spaceAvailable} space(s) remaining. You attempted to add {totalAmount} item(s). Please adjust the quantity to fit within the available space.");
                        return;
                    }
                }
            }
        });

        RuleFor(transfer => transfer.TransferStatus)
            .NotNull().WithMessage($"Invalid transfer_status. Allowed values are ({EnumUtil.EnumsToString<TransferStatus>()})");

        RuleFor(transfer => transfer.TransferFromId)
            .Custom((TransferFromId, context) =>
            {
                if (TransferFromId.HasValue && !db.Locations.Any(l => l.Id == TransferFromId))
                {
                    context.AddFailure("transfer_from_id", "The provided location of transfer_from_id does not exist.");
                }
            });

        RuleFor(transfer => transfer.TransferToId)
            .Custom((TransferToId, context) =>
            {
                if (TransferToId.HasValue && !db.Locations.Any(l => l.Id == TransferToId))
                {
                    context.AddFailure("transfer_to_id", "The provided location of transfer_to_id does not exist.");
                }
            });

        RuleForEach(transfer => transfer.TransferItems)
        .ChildRules(item =>
        {
            item.RuleFor(item => item.Amount)
                .GreaterThanOrEqualTo(1).WithMessage("amount must be at least 1.");

            item.RuleFor(item => item.ItemId)
                .NotNull().WithMessage("item_id is required.")
                .NotEmpty().WithMessage("item_id cannot be empty.")
                .Custom((itemId, context) =>
                {
                    if (itemId != null && !db.Items.Any(i => i.Id == itemId))
                    {
                        context.AddFailure("item_id", "The provided item_id does not exist.");
                    }
                });
        });


        RuleFor(transfer => transfer.TransferItems)
            .NotNull().WithMessage("items is required.")
            .NotEmpty().WithMessage("items cannot be empty.")
            .Custom((transferItems, context) =>
            {
                if (transferItems != null && CollectionUtil.ContainsDuplicateId(transferItems.Select(i => i.ItemId).ToList()))
                {
                    context.AddFailure("transfer_items must have unique item IDs. Duplicate item IDs are not allowed.");
                }
            });
    }
}