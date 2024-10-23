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

}
