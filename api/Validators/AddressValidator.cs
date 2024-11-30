using FluentValidation;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Street)
            .NotEmpty().WithMessage("street is required.")
            .NotNull().WithMessage("street cannot be null.");
        RuleFor(address => address.HouseNumber)
            .NotEmpty().WithMessage("house_number is required.")
            .NotNull().WithMessage("house_number cannot be null.");
        RuleFor(address => address.ZipCode)
            .NotEmpty().WithMessage("zip_code is required.")
            .NotNull().WithMessage("zip_code cannot be null.");
        RuleFor(address => address.City)
            .NotEmpty().WithMessage("city is required.")
            .NotNull().WithMessage("city cannot be null.");
        RuleFor(address => address.Province)
            .NotEmpty().WithMessage("province is required.")
            .NotNull().WithMessage("province cannot be null.");
        RuleFor(address => address.CountryCode)
            .NotEmpty().WithMessage("country_code is required.")
            .NotNull().WithMessage("country_code cannot be null.");
    }
}
