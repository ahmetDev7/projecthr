using FluentValidation;

public class SupplierValidator : AbstractValidator<Supplier>
{
    public SupplierValidator()
    {
        RuleFor(Supplier => Supplier.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name name cannot be empty.");
        RuleFor(Supplier => Supplier.Code)
            .NotNull().WithMessage("code is required.")
            .NotEmpty().WithMessage("code cannot be empty.");
        RuleFor(Supplier => Supplier.Reference)
            .NotNull().WithMessage("reference is required.")
            .NotEmpty().WithMessage("reference cannot be empty.");
    }
}