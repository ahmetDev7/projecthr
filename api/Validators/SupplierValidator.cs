using FluentValidation;

public class SupplierValidator : AbstractValidator<Supplier>
{
    public SupplierValidator(AppDbContext db)
    {
        RuleFor(Supplier => Supplier.Name)
            .NotNull().WithMessage("name is required.")
            .NotEmpty().WithMessage("name cannot be empty.");

        RuleFor(Supplier => Supplier.Code)
            .NotNull().WithMessage("code is required.")
            .NotEmpty().WithMessage("code cannot be empty.");

        RuleFor(Supplier => Supplier.Reference)
            .NotNull().WithMessage("reference is required.")
            .NotEmpty().WithMessage("reference cannot be empty.");

        RuleFor(Supplier => Supplier.ContactId)
            .NotNull().WithMessage("contact_id is required.")
            .NotEmpty().WithMessage("contact_id cannot be empty.")
            .Custom((ContactId, context) =>
            {
                if (!db.Contacts.Any(ig => ig.Id == ContactId)) context.AddFailure("contact_id", "The provided contact_id does not exist.");
            });

        RuleFor(Supplier => Supplier.AddressId)
            .NotNull().WithMessage("address_id is required.")
            .NotEmpty().WithMessage("address_id cannot be empty.")
            .Custom((AddressId, context) =>
             {
                 if (!db.Addresses.Any(ig => ig.Id == AddressId)) context.AddFailure("address_id", "The provided address_id does not exist.");
             });
    }
}