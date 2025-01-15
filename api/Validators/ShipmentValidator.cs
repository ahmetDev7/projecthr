using FluentValidation;

public class ShipmentValidator : AbstractValidator<Shipment>
{
    public ShipmentValidator(AppDbContext db)
    {
        //NOT NULL CHECK:
        // TODO: CREATED_BY

        RuleFor(shipment => shipment.OrderDate)
            .NotNull().WithMessage("order_date is required.")
            .NotEmpty().WithMessage("order_date cannot be empty.");

        RuleFor(shipment => shipment.RequestDate)
            .NotNull().WithMessage("request_date is required.")
            .NotEmpty().WithMessage("request_date cannot be empty.");

        RuleFor(shipment => shipment.RequestDate)
            .NotNull().WithMessage("request_date is required.")
            .NotEmpty().WithMessage("request_date cannot be empty.");

        RuleFor(shipment => shipment.ShipmentType)
            .NotNull().WithMessage("shipment_type is required.")
            .NotEmpty().WithMessage("shipment_type cannot be empty.");

        RuleFor(shipment => shipment.ShipmentStatus)
            .NotNull().WithMessage("shipment_status is required.")
            .NotEmpty().WithMessage("shipment_status cannot be empty.");

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

        RuleFor(shipment => shipment.ShipmentItems)
            .NotEmpty().WithMessage("items is required.")
            .NotNull().WithMessage("items is required.");

        RuleForEach(shipment => shipment.ShipmentItems).ChildRules(item =>
        {
            item.RuleFor(item => item.Amount)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Amount must be at least 1.");

            item.RuleFor(item => item.ItemId)
                .Custom((itemId, context) =>
                {
                    if (itemId != null && !db.Items.Any(i => i.Id == itemId))
                    {
                        context.AddFailure("item_id", "The provided item_id does not exist.");
                    }
                });
        });

        RuleForEach(shipment => shipment.OrderShipments).ChildRules(orderShipment =>
        {
            orderShipment.RuleFor(orderShipment => orderShipment.OrderId)
                .Custom((orderId, context) =>
                {
                    if (orderId != null && !db.Orders.Any(o => o.Id == orderId))
                    {
                        context.AddFailure("order_id", "The provided order_id does not exist.");
                    }
                });
        });


        //FIXME: CUSTOM RULE TO CHECKING IF SHIPMENT_ITEM HAS INVENTORY
    }
}