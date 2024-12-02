using FluentValidation;

public class ItemTypeValidator : AbstractValidator<ItemType>
{
    public ItemTypeValidator()
    {
        RuleFor(itemType => itemType.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");
    }
}