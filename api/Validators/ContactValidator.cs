using FluentValidation;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator(AppDbContext db)
    {
        RuleFor(contact => contact.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty.");

        RuleFor(contact => contact.Phone)
            .NotNull().WithMessage("Phone is required.")
            .NotEmpty().WithMessage("Phone cannot be empty.");

        RuleFor(contact => contact.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email cannot be empty.");
    }
}
