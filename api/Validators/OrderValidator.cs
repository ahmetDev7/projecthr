using System.Data;
using FluentValidation;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator(AppDbContext db)
    {
        RuleFor(Order => Order.OrderDate)
            .NotNull().WithMessage("order_date required")
            .NotEmpty().WithMessage("order_date cannot be empty.");
        RuleFor(Order => Order.OrderStatus)
            .NotNull().WithMessage("order_status required")
            .NotEmpty().WithMessage("order_status cannot be empty.");
        RuleFor(Order => Order.WarehouseId)
            .NotNull().WithMessage("warehouse_id required")
            .NotEmpty().WithMessage("warehouse_id cannot be empty.")
            .Custom((warehouseId, context ) => {
                if (!db.Warehouses.Any(w => w.Id == warehouseId))
                {
                    context.AddFailure("warehouse_id", "The provided warehouse_id does not exist");
                }
            });
        RuleFor(order => order.OrderItems)
            .NotNull().WithMessage("order_items required")
            .NotEmpty().WithMessage("order_items cannot be empty.")
            .ForEach(orderItem => {
                orderItem.ChildRules(item =>
                {
                    item.RuleFor(i => i.ItemId)
                        .NotNull().WithMessage("The ItemId field is required.")
                        .NotEmpty().WithMessage("The ItemId field cannot be empty.")
                        .Custom((itemId, context) =>
                        {
                            if (!db.Items.Any(i => i.Id == itemId))
                            {
                                context.AddFailure("ItemId", $"The ItemId '{itemId}' does not exist in the database.");
                            }
                        });
                    
                    item.RuleFor(i => i.Amount)
                        .GreaterThan(0).WithMessage("Amount must be greater than 0.");
                });
            });
        //TODO:clientsId

    }
}
