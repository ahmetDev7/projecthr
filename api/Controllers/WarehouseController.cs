using CargoHub.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


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
    public ActionResult<Warehouse> GetById(Guid id)
    {
        Warehouse? foundWarehouse = _warehouseProvider.GetById(id);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(foundWarehouse);
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<Warehouse>> GetAll()
    {
        List<Warehouse>? allWarehouses = _warehouseProvider.GetAll();
        if (allWarehouses == null) return NotFound(new { message = $"No warehouses found" });
        return Ok(allWarehouses);
    }

    [HttpGet("{warehouseId}/locations")]
    public IActionResult GetLocations(Guid warehouseId)
    {
        try
        {
            var locations = _warehouseProvider.GetLocationsByWarehouseId(warehouseId);
            return Ok(locations);
        }
        catch (ApiFlowException ex)
        {
            return NotFound(ex.Message);
        }
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

    [HttpDelete("{id}")]
    public IActionResult DeleteWarehouse(Guid id)
    {
        try
        {
            Warehouse? deletedWarehouse = _warehouseProvider.Delete(id);
            if (deletedWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
            return Ok(new { Message = "Warehouse deleted successfully!" });
        }
        catch (ApiFlowException apiFlowException)
        {
            return Problem(apiFlowException.Message, statusCode: 500);
        }
        catch (Exception)
        {
            return Problem("An error occurred while deleting a warehouse. Please try again.", statusCode: 500);
        }
    }

}