using DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class WarehousesController : ControllerBase
{
    private readonly WarehouseProvider _warehouseProvider;

    public WarehousesController(WarehouseProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;

    }

    [HttpGet("{id}")]
    public ActionResult<WarehouseResultDTO> GetById(Guid id)
    {
        var foundWarehouse = _warehouseProvider.GetByIdAsDTO(id);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok((WarehouseResultDTO)foundWarehouse);
    }

    [HttpGet("all")]
    public ActionResult<List<WarehouseResultDTO>> GetAll()
    {
        var result = _warehouseProvider.GetAll()
                                   .OfType<WarehouseResultDTO>()
                                   .ToList();
        return Ok(result);
    }

    [HttpGet("{warehouseId}/locations")]
    public IActionResult GetLocations(Guid warehouseId)
    {
        var locations = _warehouseProvider.GetLocationsByWarehouseId(warehouseId);
        return Ok(locations);
    }

    [HttpPost]
    public IActionResult CreateWarehouse([FromBody] WarehouseDTO request)
    {
        try
        {
            Warehouse? createdWarehouse = _warehouseProvider.Create<WarehouseDTO>(request);
            if (createdWarehouse == null) throw new ApiFlowException("An error occurred while creating the warehouse");
            return Ok(new { Message = "Warehouse created successfully!" });
        }
        catch (ApiFlowException apiFlowException)
        {
            return Problem(apiFlowException.Message, statusCode: 500);
        }
        catch (Exception)
        {
            return Problem("An error occurred while creating an warehouse. Please try again.", statusCode: 500);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateWarehouse(Guid id, [FromBody] WarehouseDTO request)
    {
       try
        {
            Warehouse? updatedWarehouse = _warehouseProvider.Update(id, request);

            if (updatedWarehouse == null)
            {
                return NotFound(new { message = $"Warehouse not found for id '{id}'" });
            }

            return Ok(new { message = "Warehouse updated!", updated_Warehouse = updatedWarehouse });
        }
        catch (ApiFlowException ex)
        {
            return Problem(ex.Message, statusCode: 500);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred while updating the location. Please try again.", statusCode: 500);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWarehouse(Guid id)
    {
        Warehouse? deletedWarehouse = _warehouseProvider.Delete(id);
        if (deletedWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(new { Message = "Warehouse deleted successfully!" });
    }
}