using FluentValidation;

public class WarehouseValidator : AbstractValidator<Warehouse>
{
    public WarehouseValidator(AppDbContext db)
    {
        RuleFor(Warehouse => Warehouse.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");

        RuleFor(Warehouse => Warehouse.Code)
            .NotNull().WithMessage("code is required.")
            .NotEmpty().WithMessage("code cannot be empty.");

        RuleFor(Warehouse => Warehouse.AddressId)
            .NotNull().WithMessage("address_id is required.")
            .NotEmpty().WithMessage("address_id cannot be empty.")
            .Custom((AddressId, context) =>
             {
                 if (!db.Addresses.Any(ig => ig.Id == AddressId)) context.AddFailure("address_id", "The provided address_id does not exist.");
             });
    }
}