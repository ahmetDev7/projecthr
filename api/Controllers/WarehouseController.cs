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
        try
        {
            Warehouse? warehouse = _warehouseProvider.GetById(id);
            if (warehouse == null) return NotFound(new { Message = "Warehouse not found" });
            return Ok(warehouse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
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
            Warehouse? createdWarehouse = _warehouseProvider.Create(request);
            if (createdWarehouse == null) BadRequest(new { Message = "Something went wrong while storing the warehouse." });
            return Ok(new { Message = "Warehouse created successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

}