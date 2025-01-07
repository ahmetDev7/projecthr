using FluentValidation;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(contact => contact.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(contact => contact.Function)
            .NotNull().WithMessage("Function is required.")
            .NotEmpty().WithMessage("Function cannot be empty.");

        RuleFor(contact => contact.Phone)
            .NotNull().WithMessage("Phone is required.")
            .NotEmpty().WithMessage("Phone cannot be empty.");

        RuleFor(contact => contact.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email cannot be empty.");
    }
}
