using FluentValidation;

public class OrdersProvider : BaseProvider<Order>
{
    private IValidator<Order> _orderValidator;

    public OrdersProvider(AppDbContext db, IValidator<Order> validator) : base(db)
    {
        _orderValidator = validator;
    }

    public override List<Order>? GetAll() => _db.Orders.ToList();

    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}