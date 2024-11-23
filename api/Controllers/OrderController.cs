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
}