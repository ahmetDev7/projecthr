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
        var warehouses = _warehouseProvider.Getall();
        return Ok(warehouses);
    }

    [HttpGet("{id}")]  // Dit specificeert dat {id} een routeparameter is
    public ActionResult<Warehouse> GetByID(int id)
    {
        try
        {
            var warehouse = _warehouseProvider.GetByID(id);
            return Ok(warehouse);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult CreateWarehouse([FromBody] Warehouse newWarehouse)
    {
        try
        {
            _warehouseProvider.CreateWarehouse(newWarehouse);
            return Ok(new { Message = "Warehouse created successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut]
    public IActionResult UpdateWarehouse([FromBody] Warehouse updatedWarehouse)
    {
        try
        {
            _warehouseProvider.UpdateWarehouse(updatedWarehouse);
            return Ok(new { Message = "Warehouse updated successfully!" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });  // 404 Not Found
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });  // 500 Internal Server Error
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWarehouse(int id)
    {
        try
        {
            _warehouseProvider.DeleteWarehouse(id);
            return Ok(new { Message = "Warehouse deleted successfully!" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message }); // 404 Not Found
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message }); // 500 Internal Server Error
        }
    }




}
