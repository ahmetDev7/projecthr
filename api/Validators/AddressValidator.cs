using FluentValidation;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator(AppDbContext db)
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
        RuleFor(address => address.Id)
            .Custom((addressId, context) =>
            {
                if (db.Warehouses.Any(w => w.AddressId == addressId) ||
                    db.Clients.Any(c => c.AddressId == addressId) ||
                    db.Suppliers.Any(s => s.AddressId == addressId))
                {
                    context.AddFailure("address_id", "The provided address_id is in use and cannot be modified.");
                }
            });
    }
}
