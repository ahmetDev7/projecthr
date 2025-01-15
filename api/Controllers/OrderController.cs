using System.Data;
using System.Security.Claims;
using DTO.Order;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "admin,warehousemanager,logistics,sales")]
    public IActionResult Create([FromBody] OrderRequest req)
    {
        string? role = User.FindFirst(ClaimTypes.Role)?.Value;
        req.CreatedBy = role;

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
                ShipToClientId = newOrder.ShipToClientId,
                BillToClientId = newOrder.BillToClientId,
                CreatedAt = newOrder.CreatedAt,
                UpdatedAt = newOrder.UpdatedAt,
                CreatedBy = newOrder.CreatedBy,
                Items = newOrder.OrderItems?.Select(oi => new OrderItemRequest
                {
                    ItemId = oi.ItemId,
                    Amount = oi.Amount
                }).ToList()
            }
        });
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,warehousemanager")]
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
                    ShipToClientId = deletedOrder.ShipToClientId,
                    BillToClientId = deletedOrder.BillToClientId,
                    CreatedAt = deletedOrder.CreatedAt,
                    UpdatedAt = deletedOrder.UpdatedAt,
                    CreatedBy = deletedOrder.CreatedBy,
                    Items = deletedOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            });
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,warehousemanager,logistics,sales")]
    public IActionResult Update(Guid id, [FromBody] OrderRequest req)
    {
        Order? foundOrder = _orderProvider.GetById(id);
        if (foundOrder == null)
        {
            return NotFound(new { message = $"Order not found for id '{id}'" });
        }

        if (foundOrder.OrderStatus == OrderStatus.Closed)
        {
            throw new ApiFlowException("This order has been closed and cannot be updated.", StatusCodes.Status409Conflict);
        }

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
                    ShipToClientId = updatedOrder.ShipToClientId,
                    BillToClientId = updatedOrder.BillToClientId,
                    CreatedAt = updatedOrder.CreatedAt,
                    UpdatedAt = updatedOrder.UpdatedAt,
                    CreatedBy = updatedOrder.CreatedBy,
                    Items = updatedOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            });
    }
    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,operative,supervisor,analyst,logistics,sales")]
    public IActionResult ShowAll() => Ok(_orderProvider.GetAll()?.Select(o => new OrderResponse
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
        ShipToClientId = o.ShipToClientId,
        BillToClientId = o.BillToClientId,
        CreatedAt = o.CreatedAt,
        UpdatedAt = o.UpdatedAt,
        CreatedBy = o.CreatedBy,
        Items = o.OrderItems?.Select(oi => new OrderItemRequest
        {
            ItemId = oi.ItemId,
            Amount = oi.Amount
        }).ToList()
    }).ToList());

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,operative,supervisor,analyst,logistics,sales")]
    public IActionResult ShowSingle(Guid id)
    {
        Order? foundOrder = _orderProvider.GetById(id);
        return foundOrder == null
            ? NotFound(new { message = $"Order not found for id '{id}'" })
            : Ok(
                new OrderResponse
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
                    ShipToClientId = foundOrder.ShipToClientId,
                    BillToClientId = foundOrder.BillToClientId,
                    CreatedAt = foundOrder.CreatedAt,
                    UpdatedAt = foundOrder.UpdatedAt,
                    CreatedBy = foundOrder.CreatedBy,                    
                    Items = foundOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            );
    }

    [HttpGet("{id}/items")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,operative,supervisor,analyst,logistics,sales")]
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

    [HttpPut("{id}/commit")]
    [Authorize(Roles = "admin,warehousemanager,logistics,sales")]
    public IActionResult ActionCommit(Guid id)
    {
        Order? foundOrder = _orderProvider.GetById(id);
        if (foundOrder == null) return NotFound(new { message = $"Order not found for id '{id}'" });

        if (foundOrder.OrderStatus == OrderStatus.Closed)
        {
            throw new ApiFlowException("This order has already been closed. Updates are not allowed.", StatusCodes.Status409Conflict);
        }

        Order? commitedOrder = _orderProvider.CommitOrder(foundOrder);

        return Ok(
            new
            {
                message = "Order closed!",
                commited_order = new OrderResponse
                {
                    Id = commitedOrder.Id,
                    OrderDate = commitedOrder.OrderDate,
                    RequestDate = commitedOrder.RequestDate,
                    Reference = commitedOrder.Reference,
                    ReferenceExtra = commitedOrder.ReferenceExtra,
                    OrderStatus = commitedOrder.OrderStatus,
                    Notes = commitedOrder.Notes,
                    PickingNotes = commitedOrder.PickingNotes,
                    TotalAmount = commitedOrder.TotalAmount,
                    TotalDiscount = commitedOrder.TotalDiscount,
                    TotalTax = commitedOrder.TotalTax,
                    TotalSurcharge = commitedOrder.TotalSurcharge,
                    WarehouseId = commitedOrder.WarehouseId,
                    ShipToClientId = commitedOrder.ShipToClientId,
                    BillToClientId = commitedOrder.BillToClientId,
                    CreatedAt = commitedOrder.CreatedAt,
                    UpdatedAt = commitedOrder.UpdatedAt,
                    CreatedBy = foundOrder.CreatedBy,
                    Items = commitedOrder.OrderItems?.Select(oi => new OrderItemRequest
                    {
                        ItemId = oi.ItemId,
                        Amount = oi.Amount
                    }).ToList()
                }
            }
        );
    }

}
