using DTO.Order;
using FluentValidation;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator(AppDbContext db)
    {
        RuleFor(OrderRequest => OrderRequest.OrderItems)
            .Custom((items, context) =>
            {
                if (items != null && CollectionUtil.ContainsDuplicateId(items.Select(i => i.ItemId).ToList()))
                {
                    context.AddFailure("items must have unique item IDs. Duplicate items are not allowed.");
                }
            });

        RuleFor(OrderRequest => OrderRequest.OrderItems)
            .NotNull().WithMessage("order_items required")
            .NotEmpty().WithMessage("order_items cannot be empty.");

        RuleFor(OrderRequest => OrderRequest.OrderStatus)
            .Custom((orderStatus, context) =>
            {
                if (orderStatus == OrderStatus.Closed)
                {
                    context.AddFailure("A order cannot have a status of 'Closed'. The 'Closed' status is only applicable when using the commit endpoint.");
                }
            });
    }
}