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
                    }
                });
        });
    }
}