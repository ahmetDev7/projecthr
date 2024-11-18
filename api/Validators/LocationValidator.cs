using FluentValidation;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(location => location.Row)
            .NotNull().WithMessage("row is required.")
            .NotEmpty().WithMessage("row name cannot be empty.");

        RuleFor(location => location.Rack)
            .NotNull().WithMessage("rack is required.")
            .NotEmpty().WithMessage("rack name cannot be empty.");

        RuleFor(location => location.Shelf)
            .NotNull().WithMessage("shelf is required.")
            .NotEmpty().WithMessage("shelf name cannot be empty.");

        RuleFor(location => location.WarehouseId)
            .NotNull().WithMessage("warehouse_id is required.")
            .NotEmpty().WithMessage("warehouse_id name cannot be empty.");
    }
}