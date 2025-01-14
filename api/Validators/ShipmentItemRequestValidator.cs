using DTO.Shipment;
using FluentValidation;

public class ShipmentItemRequestValidator : AbstractValidator<UpdateShipmentItemDTO>
{
    public ShipmentItemRequestValidator(AppDbContext db)
    {

        RuleFor(req => req.Items)
            .NotEmpty().WithMessage("items is required.")
            .NotNull().WithMessage("items is required.");

        // Validation to check if item is in DB is in ShipmentValidation
        RuleFor(req => req.Items)
            .Custom((items, context) =>
            {
                if (items != null && CollectionUtil.ContainsDuplicateId(items.Select(i => i.ItemId).ToList()))
                {
                    context.AddFailure("items must have unique item IDs. Duplicate items are not allowed.");
                }
            });

        RuleFor(req => req).Custom((req, context) =>
        {
            foreach (Guid? orderId in req.Orders ?? [])
            {
                if (orderId.HasValue)
                {
                    foreach (ShipmentItemRR? row in req.Items ?? [])
                    {
                        bool hasOrderItem = db.OrderItems.Any(oi => oi.OrderId == orderId && oi.ItemId == row.ItemId);
                        if (!hasOrderItem)
                        {
                            context.AddFailure("orders", $"The provided item with ID {row.ItemId} is not part of order {orderId}.");
                            return;
                        }

                        bool orderIsClosed = db.Orders.Any(o => o.OrderStatus == OrderStatus.Closed);
                        if (orderIsClosed)
                        {
                            context.AddFailure("orders", $"The order with ID {orderId} is closed. Shipments can no longer be assigned to a closed order.");
                            return;
                        }
                    }
                    // FUTURE FEATURE: shipment_item amount not exceeding order_item amount
                }
            }



        });
    }
}