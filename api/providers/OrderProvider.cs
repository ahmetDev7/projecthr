using DTO.ItemGroup;
using DTO.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Utils.Date;


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
            // TODO: calculated value
            OrderDate = DateUtil.ToUtcOrNull(req.OrderDate),
            RequestDate = DateUtil.ToUtcOrNull(req.RequestDate),
            Reference = req.Reference,
            ReferenceExtra = req.ReferenceExtra,
            OrderStatus = req.OrderStatus,
            Notes = req.Notes,
            PickingNotes = req.PickingNotes,
            WarehouseId = req.WarehouseId
        };
        
        ValidateModel(newOrder);
        _db.Orders.Add(newOrder);
        SaveToDBOrFail();
        return newOrder;
    }

    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}