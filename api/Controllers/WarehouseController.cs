
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

}
