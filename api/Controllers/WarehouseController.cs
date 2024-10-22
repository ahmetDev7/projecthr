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
}
