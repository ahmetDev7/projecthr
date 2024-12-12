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
        RuleFor(contact => contact.Id)
            .NotNull().WithMessage("contact_id is required.")
            .NotEmpty().WithMessage("contact_id cannot be empty.")
            .Custom((contactId, context) =>
            {
                if (db.Warehouses.Any(w => w.ContactId == contactId) ||
                    db.Clients.Any(c => c.ContactId == contactId) ||
                    db.Suppliers.Any(s => s.ContactId == contactId))
                {
                    context.AddFailure("contact_id", "The provided contact_id is in use and cannot be modified or deleted.");
                }
            });
    }
}
