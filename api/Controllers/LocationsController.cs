using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{

    private readonly LocationsProvider _locationProvider;

    public LocationsController(LocationsProvider locationProvider)
    {
        _locationProvider = locationProvider;
    }


    [HttpGet]
    public IActionResult GetLocations()
    {
        var locations = _locationProvider.GetAll();

        return Ok(locations);
    }


    [HttpGet("{id}")]
    public IActionResult GetLocation(Guid id)
    {
        return Ok(new { message = "Single location" });

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
    public  IActionResult DeleteLocation(Guid id)
    {
        return Ok(new { message = "Location deleted!" });

    }


    // DTO: Data Transferable Object
    public class LocationDTO {
        public string Name {get; set;}
    }
}
