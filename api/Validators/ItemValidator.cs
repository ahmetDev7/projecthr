using FluentValidation;

public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator(AppDbContext db)
    {
        RuleFor(item => item.Code)
            .NotNull().WithMessage("code is required.")
            .NotEmpty().WithMessage("code name cannot be empty.");

        RuleFor(item => item.UpcCode)
            .NotNull().WithMessage("upc_code is required.")
            .NotEmpty().WithMessage("upc_code name cannot be empty.");

        RuleFor(item => item.ModelNumber)
            .NotNull().WithMessage("model_number is required.")
            .NotEmpty().WithMessage("model_number name cannot be empty.");

        RuleFor(item => item.UnitPurchaseQuantity)
            .NotNull().WithMessage("unit_purchase_quantity is required.");
        
        RuleFor(item => item.UnitOrderQuantity)
            .NotNull().WithMessage("unit_order_quantity is required.");

        RuleFor(item => item.PackOrderQuantity)
            .NotNull().WithMessage("pack_order_quantity is required.");

        RuleFor(item => item.SupplierReferenceCode)
            .NotNull().WithMessage("supplier_reference_code is required.")
            .NotEmpty().WithMessage("supplier_reference_code name cannot be empty.");

        RuleFor(item => item.ItemGroupId)
            .Custom((itemGroupId, context) => {
                if (itemGroupId != null && !db.ItemGroups.Any(ig => ig.Id == itemGroupId))
                {
                    context.AddFailure("item_group_id", "The provided item_group_id does not exist.");
                }
            });
    }
}