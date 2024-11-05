using DTOs;
using FluentValidation;
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
        try
        {
            Location? newLocation = _locationsProvider.Create<LocationDTO>(req);
            if (newLocation == null) throw new ApiFlowException("Saving new location failed.");
            return Ok(new { message = "Location created!", new_location = newLocation });
        }
        catch (ApiFlowException apiFlowException)
        {
            return Problem(apiFlowException.Message, statusCode: 500);
        }
        catch (ValidationException ValidationException)
        {
            return BadRequest(ValidationException.Errors);
        }
        catch (Exception)
        {
            return Problem("An error occurred while creating an location. Please try again.", statusCode: 500);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLocation(Guid id, [FromBody] LocationDTO req)
    {
        try
        {
            Location? updatedLocation = _locationsProvider.Update(id, req);

            if (updatedLocation == null)
            {
                return NotFound(new { message = $"Location not found for id '{id}'" });
            }

            return Ok(new { message = "Location updated!", updated_location = updatedLocation });
        }
        catch (ApiFlowException ex)
        {
            return Problem(ex.Message, statusCode: 500);
        }
        catch (ValidationException ValidationException)
        {
            return BadRequest(ValidationException.Errors);
        }
        catch (Exception)
        {
            return Problem("An error occurred while updating the location. Please try again.", statusCode: 500);
        }


    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLocation(Guid id)
    {
        try
        {
            Location? deletedLocation = _locationsProvider.Delete(id);

            if (deletedLocation == null) return NotFound(new { message = $"Location not found for id '{id}'" });

            return Ok(new { message = "Location deleted!", deleted_location = deletedLocation });
        }
        catch (ApiFlowException apiFlowException)
        {
            return Problem(apiFlowException.Message, statusCode: 500);
        }
        catch (Exception)
        {
            return Problem("An error occurred while creating an location. Please try again.", statusCode: 500);
        }

    }
}