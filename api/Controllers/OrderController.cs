using System.Data;
using DTO.ItemGroup;
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

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Order? deletedOrder = _orderProvider.Delete(id);
        
        return deletedOrder == null
            ? NotFound(new { message = $"Order not found for id '{id}'" })
            : Ok(new
            {
                message = "Order deleted!",
                deleted_order = new OrderResponse
                {
                    Id = deletedOrder.Id,
                    OrderDate = deletedOrder.OrderDate,
                    RequestDate = deletedOrder.RequestDate,
                    Reference = deletedOrder.Reference,
                    ReferenceExtra = deletedOrder.ReferenceExtra,
                    OrderStatus = deletedOrder.OrderStatus,
                    Notes = deletedOrder.Notes,
                    PickingNotes = deletedOrder.PickingNotes,
                    TotalAmount = deletedOrder.TotalAmount,
                    TotalDiscount = deletedOrder.TotalDiscount,
                    TotalTax = deletedOrder.TotalTax,
                    TotalSurcharge = deletedOrder.TotalSurcharge,
                    WarehouseId = deletedOrder.WarehouseId,
                    CreatedAt = deletedOrder.CreatedAt,
                    UpdatedAt = deletedOrder.UpdatedAt,
                    Items = deletedOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            });
    }

}