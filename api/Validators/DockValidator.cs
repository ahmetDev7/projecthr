using FluentValidation;

public class DockValidator : AbstractValidator<Dock>
{
    public DockValidator(AppDbContext db)
    {
        RuleFor(dock => dock.WarehouseId)
            .Custom((warehouseId, context) =>
            {
                if (!db.Warehouses.Any(w => w.Id == warehouseId))
                {
                    context.AddFailure("warheouse_id", "The provided warehouse_id does not exist.");
                }
            });
    }
}

