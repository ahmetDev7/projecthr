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


    [HttpGet]
    public IActionResult GetLocations()
    {
        var locations = _locationsProvider.GetAll();

        return Ok(locations);
    }


    [HttpGet("{id}")]
    public IActionResult GetLocation(int id)
    {
        var getLocationId = _locationsProvider.GetById(id);
        return Ok(getLocationId);

    }

    [HttpPost]
    public IActionResult CreateLocation(LocationDTO location)
    {
        return Ok(new { message = "Location created!" });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLocation(Guid id, LocationDTO location)
    {
        return Ok(new { message = "Location updated!" });

    }

    [HttpDelete("{id}")]
    public  IActionResult DeleteLocation(int id)
    {
        bool deletedLocation = _locationsProvider.Delete(id);
        if (deletedLocation){
            return Ok("Location deleted!");
        }
        return Ok("Location is not deleted!");

    }


    // DTO: Data Transferable Object
    public class LocationDTO {
        public string Name {get; set;}
    }
}
