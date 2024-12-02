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
            .Custom((itemGroupId, context) =>
            {
                if (itemGroupId != null && !db.ItemGroups.Any(ig => ig.Id == itemGroupId))
                {
                    context.AddFailure("item_group_id", "The provided item_group_id does not exist.");
                }
            });

        RuleFor(item => item.ItemLineId)
            .Custom((ItemLineId, context) =>
            {
                if (ItemLineId != null && !db.ItemLines.Any(il => il.Id == ItemLineId))
                {
                    context.AddFailure("item_line_id", "The provided item_line_id does not exist.");
                }
            });

        RuleFor(item => item.ItemTypeId)
            .Custom((ItemTypeId, context) =>
            {
                if (ItemTypeId != null && !db.ItemTypes.Any(it => it.Id == ItemTypeId))
                {
                    context.AddFailure("item_type_id", "The provided item_type_id does not exist.");
                }
            });

        RuleFor(item => item.SupplierId)
            .NotNull().WithMessage("supplier_id is required.")
            .NotEmpty().WithMessage("supplier_id cannot be empty.")
            .Custom((SupplierId, context) =>
            {
                if (!db.Suppliers.Any(s => s.Id == SupplierId))
                {
                    context.AddFailure("supplier_id", "The provided supplier_id does not exist.");
                }
            });
    }
}