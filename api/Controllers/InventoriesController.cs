using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly InventoriesProvider _inventoriesProvider;

    public InventoriesController(InventoriesProvider inventoriesProvider, LocationsProvider locationsProvider)
    {
        _inventoriesProvider = inventoriesProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] InventoryRequest req)
    {
        if (req.ItemId.HasValue && _inventoriesProvider.GetInventoryByItemId(req.ItemId.Value) != null)
        {
            return BadRequest(new { message = $"Inventory already exists for ItemId '{req.ItemId.Value}'" });
        }

        Inventory? newInventory = _inventoriesProvider.Create(req);
        if (newInventory == null) throw new ApiFlowException("Inventory not found", StatusCodes.Status404NotFound);

        return Ok(
            new
            {
                message = "Inventory created!",
                created_inventory = new InventoryResponse()
                {
                    Id = newInventory.Id,
                    Description = newInventory.Description,
                    ItemReference = newInventory.ItemReference,
                    ItemId = newInventory.ItemId,
                    Locations = _inventoriesProvider.GetInventoryLocations(newInventory.Id).Select(i => new InventoryLocationResponse
                    {
                        WarehouseId = i.Location.WarehouseId,
                        LocationId = i.LocationId,
                        OnHand = i.OnHandAmount
                    }).ToList(),
                    TotalOnHand = newInventory.TotalOnHand,
                    TotalExpected = newInventory.TotalExpected,
                    TotalOrdered = newInventory.TotalOrderd,
                    TotalAllocated = newInventory.TotalAllocated,
                    TotalAvailable = newInventory.TotalAvailable,
                    CreatedAt = newInventory.CreatedAt,
                    UpdatedAt = newInventory.UpdatedAt,
                }
            }
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] InventoryRequest req)
    {
        Inventory? updatedInventory = _inventoriesProvider.Update(id, req);
        if (updatedInventory == null) throw new ApiFlowException("Inventory not found", StatusCodes.Status404NotFound);

        return Ok(
            new
            {
                message = "Inventory updated!",
                updated_inventory = new InventoryResponse()
                {
                    Id = updatedInventory.Id,
                    Description = updatedInventory.Description,
                    ItemReference = updatedInventory.ItemReference,
                    ItemId = updatedInventory.ItemId,
                    Locations = _inventoriesProvider.GetInventoryLocations(updatedInventory.Id).Select(i => new InventoryLocationResponse
                    {
                        WarehouseId = i.Location.WarehouseId,
                        LocationId = i.LocationId,
                        OnHand = i.OnHandAmount
                    }).ToList(),
                    TotalOnHand = updatedInventory.TotalOnHand,
                    TotalExpected = updatedInventory.TotalExpected,
                    TotalOrdered = updatedInventory.TotalOrderd,
                    TotalAllocated = updatedInventory.TotalAllocated,
                    TotalAvailable = updatedInventory.TotalAvailable,
                    CreatedAt = updatedInventory.CreatedAt,
                    UpdatedAt = updatedInventory.UpdatedAt,
                }
            }
        );
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Inventory? foundInventory = _inventoriesProvider.GetById(id);
        if (foundInventory == null) throw new ApiFlowException($"Inventory not found for id '{id}'", StatusCodes.Status404NotFound);

        List<InventoryLocationResponse>? inventoryLocationResponse = _inventoriesProvider.GetInventoryLocations(foundInventory.Id).Select(i => new InventoryLocationResponse
        {
            WarehouseId = i.Location.WarehouseId,
            LocationId = i.LocationId,
            OnHand = i.OnHandAmount
        }).ToList();

        Inventory? deletedInventory = _inventoriesProvider.Delete(id);
        if (deletedInventory == null) throw new ApiFlowException("Failed to delete inventory");

        return Ok(
            new
            {
                message = "Inventory deleted!",
                deleted_inventory = new InventoryResponse()
                {
                    Id = foundInventory.Id,
                    Description = foundInventory.Description,
                    ItemReference = foundInventory.ItemReference,
                    ItemId = foundInventory.ItemId,
                    Locations = inventoryLocationResponse,
                    TotalOnHand = foundInventory.TotalOnHand,
                    TotalExpected = foundInventory.TotalExpected,
                    TotalOrdered = foundInventory.TotalOrderd,
                    TotalAllocated = foundInventory.TotalAllocated,
                    TotalAvailable = foundInventory.TotalAvailable,
                    CreatedAt = foundInventory.CreatedAt,
                    UpdatedAt = foundInventory.UpdatedAt,
                }
            }
        );
    }


    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Inventory? foundInventory = _inventoriesProvider.GetById(id);
        if (foundInventory == null) throw new ApiFlowException($"Inventory not found for id '{id}'", StatusCodes.Status404NotFound);

        return Ok(new InventoryResponse()
        {
            Id = foundInventory.Id,
            Description = foundInventory.Description,
            ItemReference = foundInventory.ItemReference,
            ItemId = foundInventory.ItemId,
            Locations = _inventoriesProvider.GetInventoryLocations(foundInventory.Id).Select(i => new InventoryLocationResponse
            {
                WarehouseId = i.Location.WarehouseId,
                LocationId = i.LocationId,
                OnHand = i.OnHandAmount
            }).ToList(),
            TotalOnHand = foundInventory.TotalOnHand,
            TotalExpected = foundInventory.TotalExpected,
            TotalOrdered = foundInventory.TotalOrderd,
            TotalAllocated = foundInventory.TotalAllocated,
            TotalAvailable = foundInventory.TotalAvailable,
            CreatedAt = foundInventory.CreatedAt,
            UpdatedAt = foundInventory.UpdatedAt,
        });
    }


    [HttpGet]
    public IActionResult ShowAll() => Ok(_inventoriesProvider.GetAll().Select(i => new InventoryResponse()
    {
        Id = i.Id,
        Description = i.Description,
        ItemReference = i.ItemReference,
        ItemId = i.ItemId,
        Locations = _inventoriesProvider.GetInventoryLocations(i.Id).Select(i => new InventoryLocationResponse
        {
            WarehouseId = i.Location.WarehouseId,
            LocationId = i.LocationId,
            OnHand = i.OnHandAmount
        }).ToList(),
        TotalOnHand = i.TotalOnHand,
        TotalExpected = i.TotalExpected,
        TotalOrdered = i.TotalOrderd,
        TotalAllocated = i.TotalAllocated,
        TotalAvailable = i.TotalAvailable,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
    }).ToList());
}