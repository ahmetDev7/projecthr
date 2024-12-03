using FluentValidation;

public class TransferValidator : AbstractValidator<Transfer>
{
    public TransferValidator(AppDbContext db)
    {
        RuleFor(transfer => transfer).Custom((transfer, context) =>
        {
            // check if transfer_from and transfer_to are not the same
            if(transfer.TransferFromId == transfer.TransferToId){
                context.AddFailure("transfer_from_id and transfer_to_id", "transfer_to_id and transfer_from_id cannot be the same.");
                return;
            }

            // check if transfer_from the locations exists
            bool fromLocationExists = db.Locations.Any(l => l.Id == transfer.TransferFromId);
            if (fromLocationExists == false)
            {
                context.AddFailure("items", "The specified transfer source location (transfer_from_id) does not exist, and as a result, the location for the provided items cannot be found.");
            }

            if(transfer.TransferItems == null || transfer.TransferItems.Count == 0) return;

            foreach (TransferItem transferItem in transfer.TransferItems)
            {
                Guid? itemId = transferItem.ItemId;

                // Checks if the item_id related to the inventory_id exists
                Inventory? foundInventory = db.Inventories.FirstOrDefault(i => i.ItemId == itemId);
                if (foundInventory == null) {
                    context.AddFailure("items", $"The specified item (ID: '{itemId}') has no inventory, and therefore, no allocated location exists for this item.");
                    return;
                }
                
                 // check if the inventory_id is on the location (from_transfer_id)
                Location? locationOfTransferFrom = db.Locations.FirstOrDefault(l => l.InventoryId == foundInventory.Id && l.Id == transfer.TransferFromId);
                if (locationOfTransferFrom == null) {
                    context.AddFailure("items", $"The specified item (ID: '{itemId}') is not available at the transfer source location (Location ID: '{transfer.TransferFromId}').");
                    return;
                }

                // check if the inventory_id is null on the location (transfer_to_id)
                bool locationToIsEmpty = db.Locations.Any(l => l.Id == transfer.TransferToId && l.InventoryId == null);
                if (locationToIsEmpty == false){
                    context.AddFailure("items", "The destination location (transfer_to_id) already contains an inventory item and cannot be used for this transfer.");
                }      

                // check if the selected on_hand amount is lower or equal to the selected amount
                int? toTransferAmount = transferItem.Amount;  
                if(toTransferAmount > locationOfTransferFrom.OnHand){
                    context.AddFailure("items", "The transfer amount exceeds the available inventory at the source location. Please adjust the amount to match the on-hand quantity.");
                }

            }
        });

        RuleFor(transfer => transfer.TransferStatus)
            .NotNull().WithMessage($"Invalid transfer_status. Allowed values are ({EnumUtil.EnumsToString<TransferStatus>()})");

        RuleFor(transfer => transfer.TransferFromId)
            .NotNull().WithMessage("transfer_from_id is required.")
            .NotEmpty().WithMessage("transfer_from_id cannot be empty.")
            .Custom((TransferFromId, context) =>
            {
                if (TransferFromId != null && !db.Locations.Any(l => l.Id == TransferFromId))
                {
                    context.AddFailure("transfer_from_id", "The provided transfer_from_id does not exist.");
                }
            });

        RuleFor(transfer => transfer.TransferToId)
            .NotNull().WithMessage("transfer_to_id is required.")
            .NotEmpty().WithMessage("transfer_to_id cannot be empty.")
            .Custom((TransferToId, context) =>
            {
                if (TransferToId != null && !db.Locations.Any(l => l.Id == TransferToId))
                {
                    context.AddFailure("transfer_to_id", "The provided transfer_to_id does not exist.");
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

        RuleForEach(transfer => transfer.TransferItems).ChildRules(item =>
        {
            item.RuleFor(item => item.ItemId)
                .Custom((itemId, context) =>
                {

                });
        });
    }
}