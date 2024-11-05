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

    // GET: api/warehouses
    [HttpGet]
    public ActionResult<IEnumerable<Warehouse>> GetAll()
    {
        try
        {
            return Ok(_warehouseProvider.GetAll());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
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
    
}