using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator(AppDbContext db)
    {
        RuleFor(Order => Order.OrderDate)
            .NotNull().WithMessage("order_date required")
            .NotEmpty().WithMessage("order_date cannot be empty.");

        RuleFor(Order => Order.OrderStatus)
            .NotNull().WithMessage("order_status required")
            .NotEmpty().WithMessage("order_status cannot be empty.");

        RuleFor(Order => Order.WarehouseId)
            .NotNull().WithMessage("warehouse_id required")
            .NotEmpty().WithMessage("warehouse_id cannot be empty.")
            .Custom((warehouseId, context) =>
            {
                if (!db.Warehouses.Any(w => w.Id == warehouseId))
                {
                    context.AddFailure("warehouse_id", "The provided warehouse_id does not exist");
                }
            });

        RuleFor(order => order)
            .Custom((order, context) =>
            {
                foreach (OrderItem orderItem in order.OrderItems)
                {
                    Guid? itemId = orderItem.ItemId;

                    Inventory? foundInventory = db.Inventories
                        ?.Include(inv => inv.InventoryLocations)
                        ?.ThenInclude(il => il.Location)
                        .FirstOrDefault(inv => inv.ItemId == itemId);

                    if (foundInventory == null)
                    {
                        context.AddFailure("ItemId", $"The item with ID '{itemId}' does not have an allocated location. Items must be assigned to an inventory location before they can be selected for an order.");
                        return;
                    }

                    if (foundInventory != null)
                    {
                        if (!foundInventory.InventoryLocations.Any(il => il.Location.WarehouseId == order.WarehouseId))
                        {
                            context.AddFailure("ItemId", $"The item with ID '{itemId}' is not allocated in the same warehouse as warehouse ID '{order.WarehouseId}'.");
                            return;
                        }

                        // Get all location IDs associated with the warehouse
                        List<Guid> locationIds = db.Locations
                            .Where(loc => loc.WarehouseId == order.WarehouseId)
                            .Select(loc => loc.Id)
                            .ToList();

                        // Get all inventory locations where LocationId is in the list and InventoryId matches
                        var inventoryLocations = db.InventoryLocations.Where(il => locationIds.Contains(il.LocationId.Value) && il.InventoryId == foundInventory.Id);

                        // Calculate the sum of OnHandAmount from the filtered inventory locations
                        decimal totalOnHandAmount = inventoryLocations.Sum(il => il.OnHandAmount);

                        if (totalOnHandAmount < orderItem.Amount)
                        {
                            context.AddFailure("ItemId", $"The selected amount for item ID '{itemId}' exceeds the available on-hand quantity in the warehouse. Selected amount: {orderItem.Amount}, Available on-hand: {totalOnHandAmount}.");
                            return;
                        }
                    }
                }
            });

        RuleFor(order => order.OrderItems)
            .NotNull().WithMessage("order_items required")
            .NotEmpty().WithMessage("order_items cannot be empty.")
            .ForEach(orderItem =>
            {
                orderItem.ChildRules(item =>
                {
                    item.RuleFor(i => i.ItemId)
                        .NotNull().WithMessage("The ItemId field is required.")
                        .NotEmpty().WithMessage("The ItemId field cannot be empty.")
                        .Custom((itemId, context) =>
                        {
                            if (!db.Items.Any(i => i.Id == itemId))
                            {
                                context.AddFailure("ItemId", $"The ItemId '{itemId}' does not exist in the database.");
                                return;
                            }
                        });

                    item.RuleFor(i => i.Amount)
                        .GreaterThan(0).WithMessage("Amount must be greater than 0.");
                });
            });


        RuleFor(Order => Order.ShipToClientId)
            .Custom((shipToClientId, context) =>
            {
                if (shipToClientId.HasValue && shipToClientId != null && !db.Clients.Any(c => c.Id == shipToClientId.Value))
                {
                    context.AddFailure("ship_to_client", "The provided ship_to_client does not exist");
                }
            });

        RuleFor(Order => Order.BillToClientId)
            .NotNull().WithMessage("bill_to_client required")
            .NotEmpty().WithMessage("bill_to_client cannot be empty.")
            .Custom((billToClientId, context) =>
            {
                if (billToClientId.HasValue && !db.Clients.Any(c => c.Id == billToClientId.Value))
                {
                    context.AddFailure("bill_to_client", "The provided bill_to_client does not exist");
                }
            });

    }
}
