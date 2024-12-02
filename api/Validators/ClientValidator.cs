using FluentValidation;

public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator(AppDbContext db)
    {
        RuleFor(client => client.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");

        RuleFor(client => client.ContactId)
            .NotNull().WithMessage("Contact information is required. Please provide a valid contact_id or contact details.")
            .NotEmpty().WithMessage("Contact information cannot be empty.")
            .Custom((contactId, context) =>
            {
                if (contactId != null && !db.Contacts.Any(c => c.Id == contactId))
                {
                    context.AddFailure("contact_id", "The provided contact_id does not exist.");
                }
            });

        RuleFor(client => client.AddressId)
            .NotNull().WithMessage("Address information is required. Please provide a valid address_id or address details.")
            .NotEmpty().WithMessage("Address information cannot be empty.")
            .Custom((addressId, context) =>
            {
                if (addressId != null && !db.Addresses.Any(a => a.Id == addressId))
                {
                    context.AddFailure("address_id", "The provided address_id does not exist.");
                }
            });
    }
}
