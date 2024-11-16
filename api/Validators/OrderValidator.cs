using FluentValidation;
using Model;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(Order => Order.OrderDate)
            .NotNull().WithMessage("order date required")
            .NotEmpty().WithMessage("order date cannot be empty.");
        RuleFor(Order => Order.OrderStatus)
            .NotNull().WithMessage("order status required")
            .NotEmpty().WithMessage("order status cannot be empty.");
        //TODO:clientsId
        RuleFor(Order => Order.WarehouseId)
            .NotNull().WithMessage("warehouse id required")
            .NotEmpty().WithMessage("warehouse id cannot be empty.");

    }



}