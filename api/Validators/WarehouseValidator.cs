using FluentValidation;

public class WarehouseValidator : AbstractValidator<Warehouse>
{
    public WarehouseValidator()
    {
        RuleFor(Warehouse => Warehouse.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");
        RuleFor(Warehouse => Warehouse.Code)
            .NotNull().WithMessage("code is required.")
            .NotEmpty().WithMessage("code cannot be empty.");

        // warehouse contactid and addressid can not be made required with the validator
        // since it would make it impossible to create a warehouse without an existing contact or address
        // I will delete these notes after the message has been relayed
    }
}