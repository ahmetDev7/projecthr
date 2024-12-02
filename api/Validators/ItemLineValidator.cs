using FluentValidation;

public class ItemLineValidator : AbstractValidator<ItemLine>
{
    public ItemLineValidator()
    {
        RuleFor(itemLine => itemLine.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");
    }
}