using FluentValidation;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Street)
            .NotNull().WithMessage("Street is required.")
            .NotEmpty().WithMessage("Street cannot be empty.");
        RuleFor(address => address.HouseNumber)
            .NotNull().WithMessage("House number is required.")
            .NotEmpty().WithMessage("House number cannot be empty.");
        RuleFor(address => address.ZipCode)
            .NotNull().WithMessage("ZipCode is required.")
            .NotEmpty().WithMessage("ZipCode cannot be empty.");
        RuleFor(address => address.City)
            .NotNull().WithMessage("City is required.")
            .NotEmpty().WithMessage("City cannot be empty.");
        RuleFor(address => address.CountryCode)
            .NotNull().WithMessage("Country code is required.")
            .NotEmpty().WithMessage("Country code cannot be empty.");
    }
}
