using DTO.ItemGroup;
using DTO.Order;
using FluentValidation;
using Model;

public class OrderProvider : BaseProvider<Order>
{
    private IValidator<Order> _orderValidator;

    public OrderProvider(AppDbContext db, IValidator<Order> validator) : base(db)
    {
        _orderValidator = validator;
    }

    public override Order? Create(BaseDTO createValues)
    {
        OrderRequest? req = createValues as OrderRequest;
        if (req == null) throw new ApiFlowException("Could not process create order request. Save new order failed.");
        Order newOrder = new Order(newInstance:true)
        {
            OrderDate = req.OrderDate,
            RequestDate = req.RequestDate,
            Reference = req.Reference,
            ReferenceExtra = req.ReferenceExtra,
            OrderStatus = req.OrderStatus,
            Notes = req.Notes,
            ShipToClient = req.ShipToClient,
            PickingNotes = req.PickingNotes,
            TotalAmount = req.TotalAmount,
            TotalDiscount = req.TotalDiscount,
            TotalTax = req.TotalTax,
            TotalSurcharge = req.TotalSurcharge,
            //WarehouseId = req.WarehouseId
        };
        ValidateModel(newOrder);
        _db.Orders.Add(newOrder);
        SaveToDBOrFail();
        return newOrder;
    }

    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}