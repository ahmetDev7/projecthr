using FluentValidation;

public class InventoryValidator : AbstractValidator<Inventory>
{
    public InventoryValidator(AppDbContext db)
    {
        RuleFor(inventory => inventory.ItemId)
            .NotNull().WithMessage("item_id is required.")
            .NotEmpty().WithMessage("item_id cannot be empty.")
            .Custom((itemId, context) =>
            {
                if (itemId != null && !db.Items.Any(i => i.Id == itemId))
                {
                    context.AddFailure("item_id", "The provided item_id does not exist.");
                }
            });
    }
}

