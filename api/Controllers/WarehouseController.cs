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

}
