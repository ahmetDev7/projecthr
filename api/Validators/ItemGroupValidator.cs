using FluentValidation;

public class ItemGroupValidator : AbstractValidator<ItemGroup>
{
    public ItemGroupValidator()
    {
        RuleFor(itemGroup => itemGroup.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name name cannot be empty.");
    }
}