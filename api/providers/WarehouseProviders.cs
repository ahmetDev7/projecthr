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