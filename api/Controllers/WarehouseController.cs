using DTO;
using DTO.Address;
using DTO.Contact;
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
        var locations = _warehouseProvider.GetLocationsByWarehouseId(warehouseId);
        return Ok(locations);
    }

    [HttpPost]
    public IActionResult CreateWarehouse([FromBody] WarehouseRequest request)
    {
        Warehouse? createdWarehouse = _warehouseProvider.Create<WarehouseRequest>(request);
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
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] WarehouseRequest req){
        Warehouse? updatedWarehouse = _warehouseProvider.Update(id,req);
        if(updatedWarehouse == null) return NotFound(new {message = $"Warehouse not found for id '{id}'"});
        
        return Ok(new {
            message = "warehouse updated.", 
            updated_warehouse = new WarehouseResponse {
                Id = updatedWarehouse.Id,
                Code = updatedWarehouse.Code,
                Name = updatedWarehouse.Name,
                Contact = new ContactResponse
                {
                    Name = updatedWarehouse.Contact.Name,
                    Email = updatedWarehouse.Contact.Email,
                    Phone = updatedWarehouse.Contact.Phone
                },
                Address = new AddressResponse
                {
                    Street = updatedWarehouse.Address.Street,
                    HouseNumber = updatedWarehouse.Address.HouseNumber,
                    HouseNumberExtension = updatedWarehouse.Address.HouseNumberExtension,
                    HouseNumberExtensionExtra = updatedWarehouse.Address.HouseNumberExtensionExtra,
                    ZipCode = updatedWarehouse.Address.ZipCode,
                    City = updatedWarehouse.Address.City,
                    Province = updatedWarehouse.Address.Province,
                    CountryCode = updatedWarehouse.Address.CountryCode
                    
                },
                CreatedAt = updatedWarehouse.CreatedAt,
                UpdatedAt = updatedWarehouse.UpdatedAt
                
            }
        });
    }
}