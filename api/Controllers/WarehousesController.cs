using Microsoft.AspNetCore.Mvc;
using DTO.Contact;
using DTO.Address;

[Route("api/[controller]")]
[ApiController]
public class WarehousesController : ControllerBase
{
    private readonly WarehousesProvider _warehouseProvider;

    public WarehousesController(WarehousesProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;
    }

    [HttpPost]
    public IActionResult Create([FromBody] WarehouseRequest req)
    {
        Warehouse? createdWarehouse = _warehouseProvider.Create(req);
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
                    Contact = new ContactResponse
                    {
                        Id = createdWarehouse.ContactId,
                        Name = createdWarehouse.Contact?.Name,
                        Phone = createdWarehouse.Contact?.Phone,
                        Email = createdWarehouse.Contact?.Email,
                        CreatedAt = createdWarehouse.Contact?.CreatedAt,
                        UpdatedAt = createdWarehouse.Contact?.UpdatedAt
            
                    },
                    Address = new AddressResponse
                    {
                        Id = createdWarehouse.AddressId,
                        Street = createdWarehouse.Address?.Street,
                        HouseNumber = createdWarehouse.Address?.HouseNumber,
                        HouseNumberExtension = createdWarehouse.Address?.HouseNumberExtension,
                        HouseNumberExtensionExtra = createdWarehouse.Address?.HouseNumberExtensionExtra,
                        ZipCode = createdWarehouse.Address?.ZipCode,
                        City = createdWarehouse.Address?.City,
                        Province = createdWarehouse.Address?.Province,
                        CountryCode = createdWarehouse.Address?.CountryCode,
                        CreatedAt = createdWarehouse.Address?.CreatedAt,
                        UpdatedAt = createdWarehouse.Address?.UpdatedAt
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
                    Contact = new ContactResponse
                    {
                        Id = updatedWarehouse.ContactId,
                        Name = updatedWarehouse.Contact?.Name,
                        Phone = updatedWarehouse.Contact?.Phone,
                        Email = updatedWarehouse.Contact?.Email,
                        CreatedAt = updatedWarehouse.Contact?.CreatedAt,
                        UpdatedAt = updatedWarehouse.Contact?.UpdatedAt
                    },
                    Address = new AddressResponse
                    {
                        Id = updatedWarehouse.AddressId,
                        Street = updatedWarehouse.Address?.Street,
                        HouseNumber = updatedWarehouse.Address?.HouseNumber,
                        HouseNumberExtension = updatedWarehouse.Address?.HouseNumberExtension,
                        HouseNumberExtensionExtra = updatedWarehouse.Address?.HouseNumberExtensionExtra,
                        ZipCode = updatedWarehouse.Address?.ZipCode,
                        City = updatedWarehouse.Address?.City,
                        Province = updatedWarehouse.Address?.Province,
                        CountryCode = updatedWarehouse.Address?.CountryCode,
                        CreatedAt = updatedWarehouse.Address?.CreatedAt,
                        UpdatedAt = updatedWarehouse.Address?.UpdatedAt
                    },
                    CreatedAt = updatedWarehouse.CreatedAt,
                    UpdatedAt = updatedWarehouse.UpdatedAt
                }
            });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
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
                Contact = new ContactResponse
                {
                    Id = deletedWarehouse.ContactId,
                    Name = deletedWarehouse.Contact?.Name,
                    Phone = deletedWarehouse.Contact?.Phone,
                    Email = deletedWarehouse.Contact?.Email,
                    CreatedAt = deletedWarehouse.Contact?.CreatedAt,
                    UpdatedAt = deletedWarehouse.Contact?.UpdatedAt
                },
                Address = new AddressResponse
                {
                    Id = deletedWarehouse.AddressId,
                    Street = deletedWarehouse.Address?.Street,
                    HouseNumber = deletedWarehouse.Address?.HouseNumber,
                    HouseNumberExtension = deletedWarehouse.Address?.HouseNumberExtension,
                    HouseNumberExtensionExtra = deletedWarehouse.Address?.HouseNumberExtensionExtra,
                    ZipCode = deletedWarehouse.Address?.ZipCode,
                    City = deletedWarehouse.Address?.City,
                    Province = deletedWarehouse.Address?.Province,
                    CountryCode = deletedWarehouse.Address?.CountryCode,
                    CreatedAt = deletedWarehouse.Address?.CreatedAt,
                    UpdatedAt = deletedWarehouse.Address?.UpdatedAt
                },
                CreatedAt = deletedWarehouse.CreatedAt,
                UpdatedAt = deletedWarehouse.UpdatedAt,
            }
        });
    }

    [HttpGet("{id}")]
    public ActionResult<Warehouse> ShowSingle(Guid id)
    {
        Warehouse? foundWarehouse = _warehouseProvider.GetById(id);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(new WarehouseResponse
        {
            Id = foundWarehouse.Id,
            Code = foundWarehouse.Code,
            Name = foundWarehouse.Name,
            Contact = new ContactResponse
            {
                Id = foundWarehouse.ContactId,
                Name = foundWarehouse.Contact?.Name,
                Phone = foundWarehouse.Contact?.Phone,
                Email = foundWarehouse.Contact?.Email,
                CreatedAt = foundWarehouse.Contact?.CreatedAt,
                UpdatedAt = foundWarehouse.Contact?.UpdatedAt
            },
            Address = new AddressResponse
            {
                Id = foundWarehouse.AddressId,
                Street = foundWarehouse.Address?.Street,
                HouseNumber = foundWarehouse.Address?.HouseNumber,
                HouseNumberExtension = foundWarehouse.Address?.HouseNumberExtension,
                HouseNumberExtensionExtra = foundWarehouse.Address?.HouseNumberExtensionExtra,
                ZipCode = foundWarehouse.Address?.ZipCode,
                City = foundWarehouse.Address?.City,
                Province = foundWarehouse.Address?.Province,
                CountryCode = foundWarehouse.Address?.CountryCode,
                CreatedAt = foundWarehouse.Address?.CreatedAt,
                UpdatedAt = foundWarehouse.Address?.UpdatedAt
            },
            CreatedAt = foundWarehouse.CreatedAt,
            UpdatedAt = foundWarehouse.UpdatedAt,
        });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_warehouseProvider.GetAll().Select(w => new WarehouseResponse
    {
        Id = w.Id,
        Code = w.Code,
        Name = w.Name,
        Contact = new ContactResponse
        {
            Id = w.ContactId,
            Name = w.Contact?.Name,
            Phone = w.Contact?.Phone,
            Email = w.Contact?.Email,
            CreatedAt = w.Contact?.CreatedAt,
            UpdatedAt = w.Contact?.UpdatedAt
        },
        Address = new AddressResponse
        {
            Id = w.AddressId,
            Street = w.Address?.Street,
            HouseNumber = w.Address?.HouseNumber,
            HouseNumberExtension = w.Address?.HouseNumberExtension,
            HouseNumberExtensionExtra = w.Address?.HouseNumberExtensionExtra,
            ZipCode = w.Address?.ZipCode,
            City = w.Address?.City,
            Province = w.Address?.Province,
            CountryCode = w.Address?.CountryCode,
            CreatedAt = w.Address?.CreatedAt,
            UpdatedAt = w.Address?.UpdatedAt
            
        },
        CreatedAt = w.CreatedAt,
        UpdatedAt = w.UpdatedAt,
    }).ToList());

    [HttpGet("{warehouseId}/locations")]
    public IActionResult GetLocationsByWarehouse(Guid warehouseId) => Ok(_warehouseProvider.GetLocationsByWarehouseId(warehouseId).Select(l => new LocationResponse
    {
        Id = l.Id,
        Row = l.Row,
        Rack = l.Rack,
        Shelf = l.Shelf,
        WarehouseId = l.WarehouseId,
        CreatedAt = l.CreatedAt,
        UpdatedAt = l.UpdatedAt
    }));
}