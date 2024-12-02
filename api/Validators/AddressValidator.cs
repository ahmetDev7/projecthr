using FluentValidation;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Street)
            .NotNull().WithMessage("street is required.")
            .NotEmpty().WithMessage("street cannot be empty.");
        RuleFor(address => address.HouseNumber)
            .NotNull().WithMessage("house_number is required.")
            .NotEmpty().WithMessage("house_number cannot be empty.");
        RuleFor(address => address.ZipCode)
            .NotNull().WithMessage("zipcode is required.")
            .NotEmpty().WithMessage("zipcode cannot be empty.");
        RuleFor(address => address.City)
            .NotNull().WithMessage("city is required.")
            .NotEmpty().WithMessage("city cannot be empty.");
        RuleFor(address => address.Province)
            .NotNull().WithMessage("province is required.")
            .NotEmpty().WithMessage("province cannot be empty.");
        RuleFor(address => address.CountryCode)
            .NotNull().WithMessage("country_code is required.")
            .NotEmpty().WithMessage("country_code cannot be empty.");
    }
}
