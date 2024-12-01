using FluentValidation;

public class TransferValidator : AbstractValidator<Transfer>
{
    public TransferValidator(AppDbContext db)
    {
        RuleFor(transfer => transfer.TransferStatus)
            .NotNull().WithMessage("transfer_status is required.")
            .NotEmpty().WithMessage($"Invalid transfer_status. Allowed values are ({EnumUtil.EnumsToString<TransferStatus>()})");

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
                .GreaterThanOrEqualTo(1)
                .WithMessage("amount must be at least 1.");

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
            .Custom((transferItems, context) => {
                if(transferItems != null && CollectionUtil.ContainsDuplicateId(transferItems.Select(i => i.ItemId).ToList())) {
                    context.AddFailure("transfer_items must have unique item IDs. Duplicate item IDs are not allowed.");
                }
            });
    }
}