using DTOs;
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
    public ActionResult<Warehouse> GetById(Guid id)
    {
        Warehouse? foundWarehouse = _warehouseProvider.GetById(id);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(foundWarehouse);
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Warehouse>> GetAll()
    {
        List<Warehouse>? allWarehouses = _warehouseProvider.GetAll();
        if (allWarehouses == null) return NotFound(new { message = $"No warehouses found" });
        return Ok(allWarehouses);
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
        Warehouse? createdWarehouse = _warehouseProvider.Create<WarehouseDTO>(request);
        if (createdWarehouse == null) throw new ApiFlowException("An error occurred while creating the warehouse");
        return Ok(new { Message = "Warehouse created successfully!" });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWarehouse(Guid id)
    {
        Warehouse? deletedWarehouse = _warehouseProvider.Delete(id);
        if (deletedWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(new { Message = "Warehouse deleted successfully!" });
    }
}