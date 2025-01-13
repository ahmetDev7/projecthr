using DTO.Shipment;
using FluentValidation;

public class ShipmentRequestValidator : AbstractValidator<ShipmentRequest>
{
    public ShipmentRequestValidator(AppDbContext db)
    {
        RuleFor(ShipmentRequest => ShipmentRequest.Orders)
           .Custom((orders, context) =>
           {
               if (orders != null && CollectionUtil.ContainsDuplicateId(orders.Select(o => o).ToList()))
               {
                   context.AddFailure("Orders must have unique order IDs. Duplicate orders are not allowed.");
               }
           });

        RuleFor(ShipmentRequest => ShipmentRequest.Items)
            .Custom((items, context) =>
            {
                if (items != null && CollectionUtil.ContainsDuplicateId(items.Select(i => i.ItemId).ToList()))
                {
                    context.AddFailure("items must have unique item IDs. Duplicate items are not allowed.");
                }
            });

        RuleForEach(ShipmentRequest => ShipmentRequest.Orders).ChildRules(order =>
        {
            order.RuleFor(order => order)
                .NotNull().WithMessage("The order_id field is required.")
                .NotEmpty().WithMessage("The order_id field cannot be empty.")
                .Custom((orderId, context) =>
                {
                    if (orderId != null && !db.Orders.Any(i => i.Id == orderId))
                    {
                        context.AddFailure("orders", $"The provided order {orderId} does not exist.");
                        return;
                    }
                });
        });


        RuleFor(ShipmentRequest => ShipmentRequest).Custom((req, context) =>
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