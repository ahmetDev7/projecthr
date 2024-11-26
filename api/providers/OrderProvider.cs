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
    _db.Orders.Include(o => o.OrderItems).FirstOrDefault(order => order.Id == id);
    
    public List<OrderItem> GetRelatedOrderById(Guid id)=> 
     _db.Orders.Where(o => o.Id == id).SelectMany(o => o.OrderItems).ToList();

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

public override Order? Update(Guid id, BaseDTO updateValues)
{
    OrderRequest? req = updateValues as OrderRequest;
    if (req == null) throw new ApiFlowException("Could not process update order request. Update failed.");

    Order? existingOrder = _db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
    if (existingOrder == null) throw new ApiFlowException($"Order not found for id '{id}'");

    existingOrder.OrderDate = DateUtil.ToUtcOrNull(req.OrderDate);
    existingOrder.RequestDate = DateUtil.ToUtcOrNull(req.RequestDate);
    existingOrder.Reference = req.Reference;
    existingOrder.ReferenceExtra = req.ReferenceExtra;
    existingOrder.OrderStatus = req.OrderStatus;
    existingOrder.Notes = req.Notes;
    existingOrder.PickingNotes = req.PickingNotes;
    existingOrder.TotalAmount = req.TotalAmount;
    existingOrder.TotalDiscount = req.TotalDiscount;
    existingOrder.TotalTax = req.TotalTax;
    existingOrder.TotalSurcharge = req.TotalSurcharge;
    existingOrder.WarehouseId = req.WarehouseId;

    if (req.OrderItems != null)
    {
        _db.OrderItems.RemoveRange(existingOrder.OrderItems);

        existingOrder.OrderItems = req.OrderItems.Select(oi => new OrderItem
        {
            ItemId = oi.ItemId,
            Amount = oi.Amount
        }).ToList();
    }

    ValidateModel(existingOrder);

    _db.Orders.Update(existingOrder);
    SaveToDBOrFail();

    return existingOrder;
}


    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}