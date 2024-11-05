using Microsoft.AspNetCore.Mvc;
using Models.Location;


[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{

    private readonly LocationsProvider _locationsProvider;

    public LocationsController(LocationsProvider locationProvider)
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
        return Ok(foundLocation);
    }

    [HttpPost()]
    public IActionResult CreateLocation([FromBody] LocationDTO req)
    {
        Location? newLocation = _locationsProvider.Create<LocationDTO>(req);
        if (newLocation == null) throw new ApiFlowException("Saving new location failed.");
        return Ok(new { message = "Location created!", new_location = newLocation });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLocation(Guid id, [FromBody] LocationDTO req)
    {
        Location? updatedLocation = _locationsProvider.Update(id, req);
        if (updatedLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });
        return Ok(new { message = "Location updated!", updated_location = updatedLocation });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLocation(Guid id)
    {
        Location? deletedLocation = _locationsProvider.Delete(id);
        if (deletedLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });
        return Ok(new { message = "Location deleted!", deleted_location = deletedLocation });

    }
}
