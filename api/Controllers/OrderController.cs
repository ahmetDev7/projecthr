using System.Data;
using DTO.Order;
using Microsoft.AspNetCore.Mvc;
using Model;


[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private OrderProvider _orderProvider;
    public OrdersController(OrderProvider orderProvider)
    {
        _orderProvider = orderProvider;
    }
    
    [HttpPost()]
    public IActionResult Create([FromBody] OrderRequest req)
    {
        Order? newOrder = _orderProvider.Create(req);
        if (newOrder == null) throw new ApiFlowException("Saving new order failed.");

        return Ok(new OrderResponse{         
            Id = newOrder.Id,
            OrderDate = newOrder.OrderDate,
            RequestDate = newOrder.RequestDate,          
            Reference = newOrder.Reference,
            ReferenceExtra = newOrder.ReferenceExtra,
            OrderStatus = newOrder.OrderStatus,
            Notes = newOrder.Notes,
            PickingNotes = newOrder.PickingNotes,
            TotalAmount = newOrder.TotalAmount,
            TotalDiscount = newOrder.TotalDiscount,
            TotalTax = newOrder.TotalTax,
            TotalSurcharge = newOrder.TotalSurcharge,
            WarehouseId = newOrder.WarehouseId,
            CreatedAt = newOrder.CreatedAt,
            UpdatedAt = newOrder.UpdatedAt,
            Items = newOrder.OrderItems?.Select(oi => new OrderItemRequest
            {
                ItemId = oi.ItemId,
                Amount = oi.Amount
            }).ToList()
        });
    }
    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Order? foundOrder = _orderProvider.GetById(id);
        if(foundOrder == null) throw new ApiFlowException($"Order not found for id '{id}'");

        return Ok(new OrderResponse{         
            Id = foundOrder.Id,
            OrderDate = foundOrder.OrderDate,
            RequestDate = foundOrder.RequestDate,          
            Reference = foundOrder.Reference,
            ReferenceExtra = foundOrder.ReferenceExtra,
            OrderStatus = foundOrder.OrderStatus,
            Notes = foundOrder.Notes,
            PickingNotes = foundOrder.PickingNotes,
            TotalAmount = foundOrder.TotalAmount,
            TotalDiscount = foundOrder.TotalDiscount,
            TotalTax = foundOrder.TotalTax,
            TotalSurcharge = foundOrder.TotalSurcharge,
            WarehouseId = foundOrder.WarehouseId,
            CreatedAt = foundOrder.CreatedAt,
            UpdatedAt = foundOrder.UpdatedAt,
            Items = foundOrder.OrderItems?.Select(oi => new OrderItemRequest
            {
                ItemId = oi.ItemId,
                Amount = oi.Amount
            }).ToList()
        });
    }

    [HttpGet("{id}/items")]
    public IActionResult ShowOrderItems(Guid id)
    {
        List<OrderItem> orderItems = _orderProvider.GetRelatedOrderById(id);
        if (!orderItems.Any()) throw new ApiFlowException($"No items found for order id '{id}'");

        return Ok(orderItems.Select(oi => new OrderItemRequest
        {
            ItemId = oi.ItemId,
            Amount = oi.Amount
        }).ToList());
    }

        [HttpGet()]
    public IActionResult ShowAll() => Ok(_orderProvider?.GetAll()?.Select(o => new OrderResponse
    {        
                  
            Id = o.Id,
            OrderDate = o.OrderDate,
            RequestDate = o.RequestDate,          
            Reference = o.Reference,
            ReferenceExtra = o.ReferenceExtra,
            OrderStatus = o.OrderStatus,
            Notes = o.Notes,
            PickingNotes = o.PickingNotes,
            TotalAmount = o.TotalAmount,
            TotalDiscount = o.TotalDiscount,
            TotalTax = o.TotalTax,
            TotalSurcharge = o.TotalSurcharge,
            WarehouseId = o.WarehouseId,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
            Items = o.OrderItems?.Select(oi => new OrderItemRequest
            {
                ItemId = oi.ItemId,
                Amount = oi.Amount
            }).ToList()
    }));
}