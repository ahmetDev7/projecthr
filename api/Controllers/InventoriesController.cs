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
        if(req.ItemId.HasValue && _inventoriesProvider.GetInventoryByItemId(req.ItemId.Value) != null){
            return BadRequest(new {message = $"Inventory already exists for ItemId '{req.ItemId.Value}'"});
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
                Description = newInventory.Description,
                ItemReference = newInventory.ItemReference,
                ItemId = newInventory.ItemId,
                Locations = locations.Select(l => new InventoryLocation() {
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
}