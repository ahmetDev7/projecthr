using DTO.Location;
using Microsoft.AspNetCore.Mvc;
using Model;


[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{

    private readonly LocationProvider _locationsProvider;

    public LocationsController(LocationProvider locationProvider)
    {
        _locationsProvider = locationProvider;
    }


    [HttpGet("all")]
    public IActionResult GetLocations()
    {
        List<Location>? allLocations = _locationsProvider.GetAll();
        if (allLocations == null) return NotFound(new { message = $"No location found" });
        return Ok(allLocations);
    }


    [HttpGet("{id}")]
    public IActionResult GetLocation(Guid id)
    {
        Location? foundLocation = _locationsProvider.GetById(id);
        if (foundLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });
            return Ok( new LocationResponse{
            Id = foundLocation.Id,
            Row = foundLocation.Row,
            Rack = foundLocation.Rack,
            Shelf = foundLocation.Shelf,
            WarehouseId = foundLocation.WarehouseId
        });
    }

    [HttpPost()]
    public IActionResult CreateLocation([FromBody] LocationReqest req)
    {
        Location? newLocation = _locationsProvider.Create(req);
        if (newLocation == null) throw new ApiFlowException("Saving new location failed.");
        return Ok( new LocationResponse{
            Id = newLocation.Id,
            Row = newLocation.Row,
            Rack = newLocation.Rack,
            Shelf = newLocation.Shelf,
            WarehouseId = newLocation.WarehouseId
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLocation(Guid id, [FromBody] LocationReqest req)
    {
        Location? updatedLocation = _locationsProvider.Update(id, req);
        if (updatedLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });
        return Ok( new LocationResponse{
            Id = updatedLocation.Id,
            Row = updatedLocation.Row,
            Rack = updatedLocation.Rack,
            Shelf = updatedLocation.Shelf,
            WarehouseId = updatedLocation.WarehouseId
        });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLocation(Guid id)
    {
        Location? deletedLocation = _locationsProvider.Delete(id);
        if (deletedLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });
                return Ok( new LocationResponse{
            Id = deletedLocation.Id,
            Row = deletedLocation.Row,
            Rack = deletedLocation.Rack,
            Shelf = deletedLocation.Shelf,
            WarehouseId = deletedLocation.WarehouseId
        });

    }
}