using FluentValidation;

public class ShipmentValidator : AbstractValidator<Shipment>
{
    public ShipmentValidator(AppDbContext db)
    {
        RuleFor(shipment => shipment.OrderId)
            .NotNull().WithMessage("order_id is required.")
            .NotEmpty().WithMessage("order_id cannot be empty.")
            .Custom((orderId, context) =>
            {
                if (orderId != null && !db.Orders.Any(o => o.Id == orderId))
                {
                    context.AddFailure("order_id", "The provided order_id does not exist.");
                }
            });
        RuleFor(shipment => shipment.ShipmentType)
            .NotNull().WithMessage("shipment_type is required.")
            .NotEmpty().WithMessage("Invalid shipment_type. Allowed values are I (Inbound) or O (Outbound).");
        RuleFor(shipment => shipment.ShipmentStatus)
            .Custom((shipmentStatus, context) =>
            {
                if (shipmentStatus == null)
                {
                    context.AddFailure("shipment_status", $"Allowed values are ({EnumUtil.EnumsToString<ShipmentStatus>()})");
                }
            });
        RuleFor(shipment => shipment.CarrierCode)
            .NotNull().WithMessage("carrier_code is required.")
            .NotEmpty().WithMessage("carrier_code cannot be empty.");
        RuleFor(shipment => shipment.ServiceCode)
            .NotNull().WithMessage("service_code is required.")
            .NotEmpty().WithMessage("service_code cannot be empty.");
        RuleFor(shipment => shipment.PaymentType)
            .NotNull().WithMessage("payment_type is required.")
            .NotEmpty().WithMessage($"Invalid payment_type. Allowed values are ({EnumUtil.EnumsToString<PaymentType>()})");
        RuleFor(shipment => shipment.TransferMode)
            .NotNull().WithMessage("transfer_mode is required.")
            .NotEmpty().WithMessage($"Invalid transfer_mode. Allowed values are ({EnumUtil.EnumsToString<TransferMode>()})");
        RuleForEach(shipment => shipment.ShipmentItems).ChildRules(item =>
        {
            item.RuleFor(item => item.Amount)
                .GreaterThanOrEqualTo(1)
                .WithMessage("amount must be at least 1.");

            item.RuleFor(item => item.ItemId)
                .Custom((itemId, context) =>
                {
                    if (itemId != null && !db.Items.Any(i => i.Id == itemId))
                    {
                        context.AddFailure("item_id", "The provided item_id does not exist.");
                    }
                });
        });
    }
}