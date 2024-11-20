using FluentValidation;

public class ShipmentValidator : AbstractValidator<Shipment>
{
    public ShipmentValidator(AppDbContext db)
    {
        RuleFor(shipment => shipment.ShipmentType)
            .NotNull().WithMessage("shipment_type is required.")
            .NotEmpty().WithMessage("shipment_type cannot be empty.");
        RuleFor(shipment => shipment.CarrierCode)
            .NotNull().WithMessage("carrier_code is required.")
            .NotEmpty().WithMessage("carrier_code cannot be empty.");
        RuleFor(shipment => shipment.ServiceCode)
            .NotNull().WithMessage("service_code is required.")
            .NotEmpty().WithMessage("service_code cannot be empty.");
        RuleFor(shipment => shipment.PaymentType)
            .NotNull().WithMessage("payment_type is required.")
            .NotEmpty().WithMessage("payment_type cannot be empty.");
        RuleFor(shipment => shipment.TransferMode)
            .NotNull().WithMessage("transfer_mode is required.")
            .NotEmpty().WithMessage("transfer_mode cannot be empty.");
        RuleForEach(shipment => shipment.ShipmentItems).ChildRules(items =>
        {
            items.RuleFor(item => item.ItemId)
                .Custom((itemId, context) => {
                    if (itemId != null && !db.Items.Any(i => i.Id == itemId))
                    {
                        context.AddFailure("item_id", "The provided item_id does not exist.");
                    }
                });
        });
    }
}