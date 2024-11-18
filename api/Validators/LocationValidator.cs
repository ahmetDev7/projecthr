using FluentValidation;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator(AppDbContext db)
    {
        RuleFor(Location => Location.Rack)
            .NotNull().WithMessage("rack is required.")
            .NotEmpty().WithMessage("rack cannot be empty.");
        RuleFor(Location => Location.Row)
            .NotNull().WithMessage("row is required.")
            .NotEmpty().WithMessage("row cannot be empty.");
        RuleFor(Location => Location.Shelf)
            .NotNull().WithMessage("shelf is required.")
            .NotEmpty().WithMessage("shelf cannot be empty.");
        RuleFor(Location => Location.WarehouseId)
            .NotNull().WithMessage("warehouse_id is required.")
            .NotEmpty().WithMessage("warehouse_id cannot be empty.");
        RuleFor(Order => Order.WarehouseId)
            .NotNull().WithMessage("warehouse_id required")
            .NotEmpty().WithMessage("warehouse_id cannot be empty.")
            .Custom((warehouseId, context ) => {
                if (!db.Warehouses.Any(w => w.Id == warehouseId))
                {
                    context.AddFailure("warehouse_id", "The provided warehouse_id does not exist");
                }
            });
    }
}