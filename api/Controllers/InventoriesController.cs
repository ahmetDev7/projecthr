using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly InventoriesProvider _inventoriesProvider;
    private readonly LocationsProvider _locationsProvider;

    public InventoriesController(InventoriesProvider inventoriesProvider, LocationsProvider locationsProvider)
    {
        _inventoriesProvider = inventoriesProvider;
        _locationsProvider = locationsProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] InventoryRequest req)
    {
        if (req.ItemId.HasValue && _inventoriesProvider.GetInventoryByItemId(req.ItemId.Value) != null)
        {
            return BadRequest(new { message = $"Inventory already exists for ItemId '{req.ItemId.Value}'" });
        }

        Inventory? newInventory = _inventoriesProvider.Create(req);
        if (newInventory == null) throw new ApiFlowException("Saving new inventory failed.");

        List<Location> locations = _locationsProvider.GetLocationsByInventoryId(newInventory.Id);
        Dictionary<string, int> calculatedValues = _inventoriesProvider.GetCalculatedValues(newInventory.Id);

        return Ok(new
        {
            message = "Inventory created!",
            created_inventory = new InventoryResponse
            {
                Id = newInventory.Id,
                Description = newInventory.Description,
                ItemReference = newInventory.ItemReference,
                ItemId = newInventory.ItemId,
                Locations = locations.Select(l => new InventoryLocation()
                {
                    LocationId = l.Id,
                    OnHand = l.OnHand
                }).ToList(),
                TotalOnHand = calculatedValues["TotalOnHand"],
                TotalExpected = calculatedValues["TotalExpected"],
                TotalOrdered = calculatedValues["TotalOrdered"],
                TotalAllocated = calculatedValues["TotalAllocated"],
                TotalAvailable = calculatedValues["TotalAvailable"],
                CreatedAt = newInventory.CreatedAt,
                UpdatedAt = newInventory.UpdatedAt,
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] InventoryRequest req)
    {
        Inventory? updatedInventory = _inventoriesProvider.Update(id, req);
        if (updatedInventory == null) return NotFound(new { message = $"Inventory not found for id '{id}'" });

        List<Location> locations = _locationsProvider.GetLocationsByInventoryId(updatedInventory.Id);
        Dictionary<string, int> calculatedValues = _inventoriesProvider.GetCalculatedValues(updatedInventory.Id);

        return Ok(new
        {
            message = "Inventory updated!",
            updated_inventory = new InventoryResponse
            {
                Id = updatedInventory.Id,
                Description = updatedInventory.Description,
                ItemReference = updatedInventory.ItemReference,
                ItemId = updatedInventory.ItemId,
                Locations = locations.Select(l => new InventoryLocation()
                {
                    LocationId = l.Id,
                    OnHand = l.OnHand
                }).ToList(),
                TotalOnHand = calculatedValues["TotalOnHand"],
                TotalExpected = calculatedValues["TotalExpected"],
                TotalOrdered = calculatedValues["TotalOrdered"],
                TotalAllocated = calculatedValues["TotalAllocated"],
                TotalAvailable = calculatedValues["TotalAvailable"],
                CreatedAt = updatedInventory.CreatedAt,
                UpdatedAt = updatedInventory.UpdatedAt,
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Inventory? foundInventory = _inventoriesProvider.GetById(id);
        if (foundInventory == null) return NotFound(new { message = $"Inventory not found for id '{id}'" });

        List<Location> locations = _locationsProvider.GetLocationsByInventoryId(foundInventory.Id);
        Dictionary<string, int> calculatedValues = _inventoriesProvider.GetCalculatedValues(foundInventory.Id);

        Inventory? deletedInventory = _inventoriesProvider.Delete(id);
        if (deletedInventory == null) throw new ApiFlowException("Failed to delete inventory");

        return Ok(new
        {
            message = "Inventory deleted!",
            deleted_inventory = new InventoryResponse
            {
                Id = foundInventory.Id,
                Description = foundInventory.Description,
                ItemReference = foundInventory.ItemReference,
                ItemId = foundInventory.ItemId,
                Locations = locations.Select(l => new InventoryLocation()
                {
                    LocationId = l.Id,
                    OnHand = l.OnHand
                }).ToList(),
                TotalOnHand = calculatedValues["TotalOnHand"],
                TotalExpected = calculatedValues["TotalExpected"],
                TotalOrdered = calculatedValues["TotalOrdered"],
                TotalAllocated = calculatedValues["TotalAllocated"],
                TotalAvailable = calculatedValues["TotalAvailable"],
                CreatedAt = foundInventory.CreatedAt,
                UpdatedAt = foundInventory.UpdatedAt,
            }
        });
    }


    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Inventory? foundInventory = _inventoriesProvider.GetById(id);
        if (foundInventory == null) return NotFound(new { message = $"Inventory not found for id '{id}'" });

        Dictionary<string, int> calculatedValues = _inventoriesProvider.GetCalculatedValues(foundInventory.Id);
        List<Location> locations = _locationsProvider.GetLocationsByInventoryId(foundInventory.Id);

        return Ok(new InventoryResponse
        {
            Id = foundInventory.Id,
            Description = foundInventory.Description,
            ItemReference = foundInventory.ItemReference,
            ItemId = foundInventory.ItemId,
            Locations = locations.Select(l => new InventoryLocation()
            {
                LocationId = l.Id,
                OnHand = l.OnHand
            }).ToList(),
            TotalOnHand = calculatedValues["TotalOnHand"],
            TotalExpected = calculatedValues["TotalExpected"],
            TotalOrdered = calculatedValues["TotalOrdered"],
            TotalAllocated = calculatedValues["TotalAllocated"],
            TotalAvailable = calculatedValues["TotalAvailable"],
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
        Locations = _locationsProvider.GetLocationsByInventoryId(i.Id).Select(l => new InventoryLocation()
        {
            LocationId = l.Id,
            OnHand = l.OnHand
        }).ToList(),
        TotalOnHand = _inventoriesProvider.CalculateTotalOnHand(i.Id),
        TotalExpected = _inventoriesProvider.CalculateTotalExpected(),
        TotalOrdered = _inventoriesProvider.CalculateTotalOrdered(),
        TotalAllocated = _inventoriesProvider.CalculateTotalAllocated(),
        TotalAvailable = _inventoriesProvider.CalculateTotalAvailable(),
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
    }).ToList());
}