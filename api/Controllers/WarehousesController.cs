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
        return Ok(new WarehouseResponse
        {
            Id = foundWarehouse.Id,
            Code = foundWarehouse.Code,
            Name = foundWarehouse.Name,
            ContactId = foundWarehouse.ContactId,
            Contact = new DTO.Contact.ContactRequest
            {
                Name = foundWarehouse.Contact.Name,
                Email = foundWarehouse.Contact.Email,
                Phone = foundWarehouse.Contact.Phone,
            },
            AddressId = foundWarehouse.AddressId,
            Address = new DTO.Address.AddressRequest
            {
                Street = foundWarehouse.Address.Street,
                HouseNumber = foundWarehouse.Address.HouseNumber,
                HouseNumberExtension = foundWarehouse.Address.HouseNumberExtension,
                HouseNumberExtensionExtra = foundWarehouse.Address.HouseNumberExtensionExtra,
                ZipCode = foundWarehouse.Address.ZipCode,
                City = foundWarehouse.Address.City,
                Province = foundWarehouse.Address.Province,
                CountryCode = foundWarehouse.Address.CountryCode,
            },
            CreatedAt = foundWarehouse.CreatedAt,
            UpdatedAt = foundWarehouse.UpdatedAt,
        });
    }

    [HttpGet()]
    public IActionResult GetAll() => Ok(_warehouseProvider.GetAll().Select(w => new WarehouseResponse
    {
        Id = w.Id,
        Code = w.Code,
        Name = w.Name,
        ContactId = w.ContactId,
        Contact = new DTO.Contact.ContactRequest
        {
            Name = w.Contact.Name,
            Email = w.Contact.Email,
            Phone = w.Contact.Phone,
        },
        AddressId = w.AddressId,
        Address = new DTO.Address.AddressRequest
        {
            Street = w.Address.Street,
            HouseNumber = w.Address.HouseNumber,
            HouseNumberExtension = w.Address.HouseNumberExtension,
            HouseNumberExtensionExtra = w.Address.HouseNumberExtensionExtra,
            ZipCode = w.Address.ZipCode,
            City = w.Address.City,
            Province = w.Address.Province,
            CountryCode = w.Address.CountryCode,
        },
        CreatedAt = w.CreatedAt,
        UpdatedAt = w.UpdatedAt,
    }).ToList());

    [HttpGet("{warehouseId}/locations")]
    public IActionResult GetLocations(Guid warehouseId) => Ok(_warehouseProvider.GetLocationsByWarehouseId(warehouseId).Select(l => new LocationResponse
    {
        Id = l.Id,
        Row = l.Row,
        Rack = l.Rack,
        Shelf = l.Shelf,
        WarehouseId = l.WarehouseId,
        CreatedAt = l.CreatedAt,
        UpdatedAt = l.UpdatedAt
    }));

    [HttpPost]
    public IActionResult CreateWarehouse([FromBody] WarehouseRequest request)
    {
        Warehouse? createdWarehouse = _warehouseProvider.Create<WarehouseRequest>(request);
        if (createdWarehouse == null) throw new ApiFlowException("Saving new warehouse failed");
        return Ok(
            new
            {
                message = "Warehouse created!",
                new_warehouse = new WarehouseResponse
                {
                    Id = createdWarehouse.Id,
                    Code = createdWarehouse.Code,
                    Name = createdWarehouse.Name,
                    ContactId = createdWarehouse.ContactId,
                    Contact = new DTO.Contact.ContactRequest
                    {
                        Name = createdWarehouse.Contact.Name,
                        Email = createdWarehouse.Contact.Email,
                        Phone = createdWarehouse.Contact.Phone,
                    },
                    AddressId = createdWarehouse.AddressId,
                    Address = new DTO.Address.AddressRequest
                    {
                        Street = createdWarehouse.Address.Street,
                        HouseNumber = createdWarehouse.Address.HouseNumber,
                        HouseNumberExtension = createdWarehouse.Address.HouseNumberExtension,
                        HouseNumberExtensionExtra = createdWarehouse.Address.HouseNumberExtensionExtra,
                        ZipCode = createdWarehouse.Address.ZipCode,
                        City = createdWarehouse.Address.City,
                        Province = createdWarehouse.Address.Province,
                        CountryCode = createdWarehouse.Address.CountryCode,
                    },
                    CreatedAt = createdWarehouse.CreatedAt,
                    UpdatedAt = createdWarehouse.UpdatedAt,
                }
            }
        );
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] WarehouseRequest req)
    {
        Warehouse? updatedWarehouse = _warehouseProvider.Update(id, req);

        return updatedWarehouse == null
            ? NotFound(new { message = $"Warehouse not found for id '{id}'" })
            : Ok(new
            {
                message = "Warehouse updated!",
                updated_warehouse = new WarehouseResponse
                {
                    Id = id,
                    Code = updatedWarehouse.Code,
                    Name = updatedWarehouse.Name,
                    ContactId = updatedWarehouse.ContactId,
                    Contact = new DTO.Contact.ContactRequest
                    {
                        Name = updatedWarehouse.Contact.Name,
                        Phone = updatedWarehouse.Contact.Phone,
                        Email = updatedWarehouse.Contact.Email
                    },
                    AddressId = updatedWarehouse.AddressId,
                    Address = new DTO.Address.AddressRequest
                    {
                        Street= updatedWarehouse.Address.Street,
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

    [HttpDelete("{id}")]
    public IActionResult DeleteWarehouse(Guid id)
    {
        Warehouse? deletedWarehouse = _warehouseProvider.Delete(id);
        if (deletedWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(new
        {
            message = "Warehouse deleted!",
            deleted_warehouse = new WarehouseResponse
            {
                Id = deletedWarehouse.Id,
                Code = deletedWarehouse.Code,
                Name = deletedWarehouse.Name,
                ContactId = deletedWarehouse.ContactId,
                Contact = new DTO.Contact.ContactRequest
                {
                    Name = deletedWarehouse.Contact.Name,
                    Email = deletedWarehouse.Contact.Email,
                    Phone = deletedWarehouse.Contact.Phone,
                },
                AddressId = deletedWarehouse.AddressId,
                Address = new DTO.Address.AddressRequest
                {
                    Street = deletedWarehouse.Address.Street,
                    HouseNumber = deletedWarehouse.Address.HouseNumber,
                    HouseNumberExtension = deletedWarehouse.Address.HouseNumberExtension,
                    HouseNumberExtensionExtra = deletedWarehouse.Address.HouseNumberExtensionExtra,
                    ZipCode = deletedWarehouse.Address.ZipCode,
                    City = deletedWarehouse.Address.City,
                    Province = deletedWarehouse.Address.Province,
                    CountryCode = deletedWarehouse.Address.CountryCode,
                },
                CreatedAt = deletedWarehouse.CreatedAt,
                UpdatedAt = deletedWarehouse.UpdatedAt,
            }
        });
    }
}