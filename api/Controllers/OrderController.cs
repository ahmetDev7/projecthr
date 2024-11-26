using System.Data;
using DTO.ItemGroup;
using DTO.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly OrderProvider _orderProvider;

    public OrdersController(OrderProvider orderProvider)
    {
        _orderProvider = orderProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] OrderRequest req)
    {
        Order? newOrder = _orderProvider.Create(req);
        if (newOrder == null) throw new ApiFlowException("Saving new order failed.");

        return Ok(new
        {
            message = "Order created!",
            new_order = new OrderResponse
            {
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
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] OrderRequest req)
    {
        Order? updatedOrder = _orderProvider.Update(id, req);

        return updatedOrder == null
            ? NotFound(new { message = $"Order not found for id '{id}'" })
            : Ok(new
            {
                message = "Order updated!",
                updated_order = new OrderResponse
                {
                    Id = updatedOrder.Id,
                    OrderDate = updatedOrder.OrderDate,
                    RequestDate = updatedOrder.RequestDate,
                    Reference = updatedOrder.Reference,
                    ReferenceExtra = updatedOrder.ReferenceExtra,
                    OrderStatus = updatedOrder.OrderStatus,
                    Notes = updatedOrder.Notes,
                    PickingNotes = updatedOrder.PickingNotes,
                    TotalAmount = updatedOrder.TotalAmount,
                    TotalDiscount = updatedOrder.TotalDiscount,
                    TotalTax = updatedOrder.TotalTax,
                    TotalSurcharge = updatedOrder.TotalSurcharge,
                    WarehouseId = updatedOrder.WarehouseId,
                    CreatedAt = updatedOrder.CreatedAt,
                    UpdatedAt = updatedOrder.UpdatedAt,
                    Items = updatedOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Order? foundOrder = _orderProvider.GetById(id);
        return foundOrder == null
            ? NotFound(new { message = $"Order not found for id '{id}'" })
            : Ok(new
            {
                message = "Order found!",
                order = new OrderResponse
                {
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
                }
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
}

