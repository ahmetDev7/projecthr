using DTO;
using Microsoft.AspNetCore.Components.Web;
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
    public ActionResult<IEnumerable<Warehouse>> GetAll() => Ok(_warehouseProvider.GetAll().Select(wh => new WarehouseResponse
    {
        Id = wh.Id,
        Code = wh.Code,
        Name = wh.Name,

        Contact = new ContactDTO
        {
            Name = wh.Contact.Name,
            Email = wh.Contact.Email,
            Phone = wh.Contact.Phone
        },
        Address = new AddressDTO
        {
            Street = wh.Address.Street,
            HouseNumber = wh.Address.HouseNumber,
            HouseNumberExtension = wh.Address.HouseNumberExtension,
            HouseNumberExtensionExtra = wh.Address.HouseNumberExtensionExtra,
            ZipCode = wh.Address.ZipCode,
            City = wh.Address.City,
            Province = wh.Address.Province,
            CountryCode = wh.Address.CountryCode,
        },
        CreatedAt = wh.CreatedAt,
        UpdatedAt = wh.UpdatedAt
    }));

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
            message = "Warehouse updated.", 
            updated_warehouse = new WarehouseResponse {
                Id = updatedWarehouse.Id,
                Code = updatedWarehouse.Code,
                Name = updatedWarehouse.Name,
                Contact = new ContactDTO
                {
                    Name = updatedWarehouse.Contact.Name,
                    Email = updatedWarehouse.Contact.Email,
                    Phone = updatedWarehouse.Contact.Phone
                },
                Address = new AddressDTO
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