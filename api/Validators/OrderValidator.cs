using System.Data;
using FluentValidation;
using Model;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator(AppDbContext db)
    {
        RuleFor(Order => Order.OrderDate)
            .NotNull().WithMessage("order date required")
            .NotEmpty().WithMessage("order date cannot be empty.");
        RuleFor(Order => Order.OrderStatus)
            .NotNull().WithMessage("order status required")
            .NotEmpty().WithMessage("order status cannot be empty.");
        RuleFor(Order => Order.WarehouseId)
            .NotNull().WithMessage("warehouse id required")
            .NotEmpty().WithMessage("warehouse id cannot be empty.")
            .Custom((warehouseId, context ) => {
                if (!db.Warehouses.Any(w => w.Id == warehouseId))
                {
                    context.AddFailure("warehouse_id", "The provided warehouse_id does not exist");
                }
            });
        //TODO:clientsId

    }
}