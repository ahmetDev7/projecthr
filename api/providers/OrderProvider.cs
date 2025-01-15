using System.Data;
using DTO.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utils.Date;


public class OrderProvider : BaseProvider<Order>
{
    private IValidator<OrderRequest> _orderRequestValidator;
    private IValidator<Order> _orderValidator;
    private readonly InventoriesProvider _inventoriesProvider;


    public OrderProvider(AppDbContext db, IValidator<Order> validator, IValidator<OrderRequest> orValidator, InventoriesProvider inventoriesProvider) : base(db)
    {
        _orderValidator = validator;
        _orderRequestValidator = orValidator;
        _inventoriesProvider = inventoriesProvider;
    }

    public override List<Order>? GetAll() => _db.Orders.Include(o => o.OrderItems).ToList();
    public override Order? GetById(Guid id) => _db.Orders.Include(o => o.OrderItems).FirstOrDefault(order => order.Id == id);

    public List<OrderItem> GetRelatedOrderById(Guid id) => _db.Orders.Where(o => o.Id == id).SelectMany(o => o.OrderItems).ToList();

    public override Order? Create(BaseDTO createValues)
    {
        OrderRequest? req = createValues as OrderRequest;
        if (req == null) throw new ApiFlowException("Could not process create order request. Save new order failed.");

        _orderRequestValidator.ValidateAndThrow(req);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Order newOrder = new Order(newInstance: true)
            {
                OrderDate = DateUtil.ToUtcOrNull(req.OrderDate),
                RequestDate = DateUtil.ToUtcOrNull(req.RequestDate),
                Reference = req.Reference,
                ReferenceExtra = req.ReferenceExtra,
                OrderStatus = req.OrderStatus,
                Notes = req.Notes,
                PickingNotes = req.PickingNotes,
                TotalAmount = req.TotalAmount,
                TotalDiscount = req.TotalDiscount,
                TotalTax = req.TotalTax,
                TotalSurcharge = req.TotalSurcharge,
                WarehouseId = req.WarehouseId,
                BillToClientId = req.BillToClientId,
                ShipToClientId = req.ShipToClientId,
                CreatedBy = req.CreatedBy,
                OrderItems = req.OrderItems?.Select(oi => new OrderItem(newInstance: true)
                {
                    ItemId = oi.ItemId,
                    Amount = oi.Amount
                }).ToList()
            };

            if (newOrder.OrderDate == null)
            {
                newOrder.OrderDate = DateUtil.ToUtcOrNull(newOrder.CreatedAt);
            }

            if (newOrder.RequestDate == null)
            {
                newOrder.RequestDate = DateUtil.ToUtcOrNull(newOrder.OrderDate);
            }

            if (newOrder.OrderStatus == null)
            {
                newOrder.OrderStatus = OrderStatus.Pending;
            }

            ValidateModel(newOrder);
            _db.Orders.Add(newOrder);
            SaveToDBOrFail();

            // TOTAL_ORDERD TRIGGERD IF STATUS PENDING
            foreach (var row in newOrder?.OrderItems ?? [])
            {
                if (row.ItemId.HasValue) _inventoriesProvider.CalculateTotalOrderd(row.ItemId.Value);
            }

            transaction.Commit();
            return newOrder;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public override Order? Delete(Guid id)
    {
        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Order? foundOrder = GetById(id);
            ICollection<OrderItem>? oldOrderItems = foundOrder?.OrderItems;
            if (foundOrder == null) return null;

            _db.Orders.Remove(foundOrder);
            SaveToDBOrFail();


            foreach (var row in oldOrderItems ?? [])
            {
                if (row.ItemId.HasValue)
                {
                    _inventoriesProvider.CalculateTotalOrderd(row.ItemId.Value); // TOTAL_ORDERD TRIGGERD IF STATUS PENDING
                    _inventoriesProvider.CalculateTotalAllocated(row.ItemId.Value); // TOTAL_ALLOCATED TRIGGERD IF STATUS CLOSED AND SHIPMENT IS SHIPMENT_STATUS.TRANSIT
                }
            }

            transaction.Commit();
            return foundOrder;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public override Order? Update(Guid id, BaseDTO updateValues)
    {
        OrderRequest? req = updateValues as OrderRequest;
        if (req == null) throw new ApiFlowException("Could not process update order request. Update failed.");
        _orderRequestValidator.ValidateAndThrow(req);

        Order? existingOrder = _db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {

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
            existingOrder.BillToClientId = req.BillToClientId;
            existingOrder.ShipToClientId = req.ShipToClientId;
            existingOrder.WarehouseId = req.WarehouseId;
            existingOrder.SetUpdatedAt();

            if (existingOrder.OrderDate == null)
            {
                existingOrder.OrderDate = DateUtil.ToUtcOrNull(existingOrder.CreatedAt);
            }

            if (existingOrder.RequestDate == null)
            {
                existingOrder.RequestDate = DateUtil.ToUtcOrNull(existingOrder.OrderDate);
            }

            if (existingOrder.OrderStatus == null)
            {
                existingOrder.OrderStatus = OrderStatus.Pending;
            }

            ICollection<OrderItem>? oldOrderItems = existingOrder.OrderItems;
            _db.OrderItems.RemoveRange(existingOrder.OrderItems);

            if (req.OrderItems != null)
            {
                existingOrder.OrderItems = req.OrderItems.Select(oi => new OrderItem(newInstance: true)
                {
                    ItemId = oi.ItemId,
                    Amount = oi.Amount
                }).ToList();
            }

            ValidateModel(existingOrder);
            _db.Orders.Update(existingOrder);
            SaveToDBOrFail();

            // TOTAL_ORDERD TRIGGERD IF STATUS PENDING
            foreach (OrderItem? row in existingOrder.OrderItems ?? [])
            {
                if (row.ItemId.HasValue) _inventoriesProvider.CalculateTotalOrderd(row.ItemId.Value);
            }

            //  TOTAL_ORDERD TRIGGERD IF STATUS PENDING [FOR OLD ORDER ITEMS]
            var removedOrderItems = oldOrderItems?.Where(oldOrderItem => !existingOrder.OrderItems.Any(newItem => newItem.ItemId == oldOrderItem.ItemId)).ToList();
            foreach (OrderItem? row in removedOrderItems ?? [])
            {
                if (row.ItemId.HasValue) _inventoriesProvider.CalculateTotalOrderd(row.ItemId.Value);
            }

            transaction.Commit();
            return existingOrder;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Order? CommitOrder(Order order)
    {
        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            order.OrderStatus = OrderStatus.Closed;
            _db.Orders.Update(order);
            SaveToDBOrFail();


            foreach (OrderItem? row in order.OrderItems ?? [])
            {
                if (row.ItemId.HasValue)
                {
                    _inventoriesProvider.CalculateTotalOrderd(row.ItemId.Value); // TOTAL_ORDERD TRIGGERD IF STATUS Changes to other status
                    _inventoriesProvider.CalculateTotalAllocated(row.ItemId.Value); // TOTAL_ALLOCATED TRIGGERD IF STATUS CLOSED AND SHIPMENT IS SHIPMENT_STATUS.TRANSIT
                }
            }

            transaction.Commit();
            return order;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    protected override void ValidateModel(Order model) => _orderValidator.ValidateAndThrow(model);
}