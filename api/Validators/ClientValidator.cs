using FluentValidation;

public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(client => client.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty.");
        RuleFor(client => client.ContactId)
            .NotNull().WithMessage("Contact ID is required.")
            .NotEmpty().WithMessage("Contact ID cannot be empty.");
        RuleFor(client => client.AddressId)
            .NotNull().WithMessage("Address ID is required.")
            .NotEmpty().WithMessage("Address ID cannot be empty.");
    }
}
