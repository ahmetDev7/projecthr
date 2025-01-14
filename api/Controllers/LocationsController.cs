using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{

    private readonly LocationsProvider _locationsProvider;

    public LocationsController(LocationsProvider locationProvider)
    {
        _locationsProvider = locationProvider;
    }

    [HttpPost()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager")]
    public IActionResult Create([FromBody] LocationRequest req)
    {
        Location? newLocation = _locationsProvider.Create(req);
        if (newLocation == null) throw new ApiFlowException("Saving new location failed.");
        return Ok(new
        {
            message = "Location created!",
            new_location = new LocationResponse
            {
                Id = newLocation.Id,
                Row = newLocation.Row,
                Rack = newLocation.Rack,
                Shelf = newLocation.Shelf,
                WarehouseId = newLocation.WarehouseId,
                CreatedAt = newLocation.CreatedAt,
                UpdatedAt = newLocation.UpdatedAt,
            }
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager")]
    public IActionResult Update(Guid id, [FromBody] LocationRequest req)
    {
        Location? updatedLocation = _locationsProvider.Update(id, req);

        return updatedLocation == null
            ? NotFound(new { message = $"Location not found for id '{id}'" })
            : Ok(new
            {
                message = "Location updated!",
                updated_location = new LocationResponse
                {
                    Id = updatedLocation.Id,
                    Row = updatedLocation.Row,
                    Rack = updatedLocation.Rack,
                    Shelf = updatedLocation.Shelf,
                    WarehouseId = updatedLocation.WarehouseId,
                    CreatedAt = updatedLocation.CreatedAt,
                    UpdatedAt = updatedLocation.UpdatedAt,
                }
            });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager")]
    public IActionResult Delete(Guid id)
    {
        Location? deletedLocation = _locationsProvider.Delete(id);
        return deletedLocation == null
            ? NotFound(new { message = $"Location not found for id '{id}'" })
            : Ok(new
            {
                message = "Location deleted!",
                deleted_location = new LocationResponse
                {
                    Id = deletedLocation.Id,
                    Row = deletedLocation.Row,
                    Rack = deletedLocation.Rack,
                    Shelf = deletedLocation.Shelf,
                    WarehouseId = deletedLocation.WarehouseId,
                    CreatedAt = deletedLocation.CreatedAt,
                    UpdatedAt = deletedLocation.UpdatedAt,
                }
            });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,operative,supervisor,analyst,logistics,sales")]
    public IActionResult ShowSingle(Guid id)
    {
        Location? foundLocation = _locationsProvider.GetById(id);
        return foundLocation == null
            ? NotFound(new { message = $"Location not found for id '{id}'" })
            : Ok(new
            {
                message = "Location found!",
                Location = new LocationResponse
                {
                    Id = foundLocation.Id,
                    Row = foundLocation.Row,
                    Rack = foundLocation.Rack,
                    Shelf = foundLocation.Shelf,
                    WarehouseId = foundLocation.WarehouseId,
                    CreatedAt = foundLocation.CreatedAt,
                    UpdatedAt = foundLocation.UpdatedAt,
                }
            });
    }

    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,operative,supervisor,analyst,logistics,sales")]
    public IActionResult ShowAll() => Ok(_locationsProvider.GetAll().Select(l => new LocationResponse
    {
        Id = l.Id,
        Row = l.Row,
        Rack = l.Rack,
        Shelf = l.Shelf,
        WarehouseId = l.WarehouseId,
        CreatedAt = l.CreatedAt,
        UpdatedAt = l.UpdatedAt,
    }).ToList());
}