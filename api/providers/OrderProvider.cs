using System.Data;
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
    public override Order? GetById(Guid id)=>
    _db.Orders.Include(s => s.OrderItems).FirstOrDefault(s => s.Id == id);
    public override Order? Create(BaseDTO createValues)
    {
        OrderRequest? req = createValues as OrderRequest;
        if (req == null) throw new ApiFlowException("Could not process create order request. Save new order failed.");
        Order newOrder = new Order(newInstance:true)
        {
            OrderDate = DateUtil.ToUtcOrNull(req.OrderDate),
            RequestDate = DateUtil.ToUtcOrNull(req.RequestDate),
            Reference = req.Reference,
            ReferenceExtra = req.ReferenceExtra,
            OrderStatus = req.OrderStatus,
            Notes = req.Notes,
            PickingNotes = req.PickingNotes,
            TotalAmount =  req.TotalAmount,
            TotalDiscount =  req.TotalDiscount,
            TotalTax = req.TotalTax,
            TotalSurcharge = req.TotalSurcharge,
            WarehouseId = req.WarehouseId,
            OrderItems = req.OrderItems?.Select(oi => new OrderItem
            {
                ItemId = oi.ItemId,
                Amount = oi.Amount
            }).ToList()
        };
        
        ValidateModel(newOrder);
        _db.Orders.Add(newOrder);
        SaveToDBOrFail();
        return newOrder;
    }

    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}