using System.Data;
using DTO.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private OrdersProvider _orderProvider;
    public OrdersController(OrdersProvider orderProvider)
    {
        _orderProvider = orderProvider;
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
        WarehouseId = o.WarehouseId,
        Items = o.OrderItems?.Select(oi => new OrderItemRequest
        {
            ItemId = oi.ItemId,
            Amount = oi.Amount
        }).ToList()
    }));

}