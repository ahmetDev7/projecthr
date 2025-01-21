using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "admin,warehousemanager")]
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
                    Contacts = createdWarehouse.WarehouseContacts.Select(wc => new ContactResponse
                    {
                        Id = wc.ContactId,
                        Name = wc.Contact?.Name,
                        Function = wc.Contact?.Function,
                        Phone = wc.Contact?.Phone,
                        Email = wc.Contact?.Email,
                        CreatedAt = wc.Contact?.CreatedAt,
                        UpdatedAt = wc.Contact?.UpdatedAt
                    }).ToList(),
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
                    Dock = new DockResponse
                    {
                        Id = createdWarehouse?.Dock?.Id,
                        CreatedAt = createdWarehouse?.Dock?.CreatedAt,
                        UpdatedAt = createdWarehouse?.Dock?.UpdatedAt,
                    },
                    CreatedAt = createdWarehouse?.CreatedAt,
                    UpdatedAt = createdWarehouse?.UpdatedAt,
                }
            }
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
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
                    Contacts = updatedWarehouse.WarehouseContacts.Select(wc => new ContactResponse
                    {
                        Id = wc.ContactId,
                        Name = wc.Contact?.Name,
                        Function = wc.Contact?.Function,
                        Phone = wc.Contact?.Phone,
                        Email = wc.Contact?.Email,
                        CreatedAt = wc.Contact?.CreatedAt,
                        UpdatedAt = wc.Contact?.UpdatedAt
                    }).ToList(),
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
                    Dock = new DockResponse
                    {
                        Id = updatedWarehouse?.Dock?.Id,
                        CreatedAt = updatedWarehouse?.Dock?.CreatedAt,
                        UpdatedAt = updatedWarehouse?.Dock?.UpdatedAt,
                    },
                    CreatedAt = updatedWarehouse?.CreatedAt,
                    UpdatedAt = updatedWarehouse?.UpdatedAt
                }
            });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
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
                Contacts = deletedWarehouse.WarehouseContacts.Select(wc => new ContactResponse
                {
                    Id = wc.ContactId,
                    Name = wc.Contact?.Name,
                    Function = wc.Contact?.Function,
                    Phone = wc.Contact?.Phone,
                    Email = wc.Contact?.Email,
                    CreatedAt = wc.Contact?.CreatedAt,
                    UpdatedAt = wc.Contact?.UpdatedAt
                }).ToList(),
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
                Dock = new DockResponse
                {
                    Id = deletedWarehouse?.Dock?.Id,
                    CreatedAt = deletedWarehouse?.Dock?.CreatedAt,
                    UpdatedAt = deletedWarehouse?.Dock?.UpdatedAt,
                },
                CreatedAt = deletedWarehouse?.CreatedAt,
                UpdatedAt = deletedWarehouse?.UpdatedAt,
            }
        });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,analyst,logistics,sales")]
    public ActionResult<Warehouse> ShowSingle(Guid id)
    {
        Warehouse? foundWarehouse = _warehouseProvider.GetById(id);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{id}'" });
        return Ok(new
        {
            message = "Warehouse retrieved successfully!",
            Warehouse = new WarehouseResponse
            {
                Id = foundWarehouse.Id,
                Code = foundWarehouse.Code,
                Name = foundWarehouse.Name,
                Contacts = foundWarehouse.WarehouseContacts.Select(wc => new ContactResponse
                {
                    Id = wc.ContactId,
                    Name = wc.Contact?.Name,
                    Function = wc.Contact?.Function,
                    Phone = wc.Contact?.Phone,
                    Email = wc.Contact?.Email,
                    CreatedAt = wc.Contact?.CreatedAt,
                    UpdatedAt = wc.Contact?.UpdatedAt
                }).ToList(),
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
                Dock = new DockResponse
                {
                    Id = foundWarehouse.Dock?.Id,
                    CreatedAt = foundWarehouse.Dock?.CreatedAt,
                    UpdatedAt = foundWarehouse.Dock?.UpdatedAt,
                },
                CreatedAt = foundWarehouse.CreatedAt,
                UpdatedAt = foundWarehouse.UpdatedAt,
            }
        });
    }

    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,analyst,logistics,sales")]
    public IActionResult ShowAll() => Ok(_warehouseProvider.GetAll().Select(w => new WarehouseResponse
    {
        Id = w.Id,
        Code = w.Code,
        Name = w.Name,
        Contacts = w.WarehouseContacts.Select(wc => new ContactResponse
        {
            Id = wc.ContactId,
            Name = wc.Contact?.Name,
            Function = wc.Contact?.Function,
            Phone = wc.Contact?.Phone,
            Email = wc.Contact?.Email,
            CreatedAt = wc.Contact?.CreatedAt,
            UpdatedAt = wc.Contact?.UpdatedAt
        }).ToList(),
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
        Dock = new DockResponse
        {
            Id = w?.Dock?.Id,
            CreatedAt = w?.Dock?.CreatedAt,
            UpdatedAt = w?.Dock?.UpdatedAt,
        },
        CreatedAt = w?.CreatedAt,
        UpdatedAt = w?.UpdatedAt,
    }).ToList());

    [HttpGet("{warehouseId}/locations")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,analyst,logistics,sales")]
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

    [HttpGet("{warehouseId}/dock")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,analyst,logistics,sales")]
    public IActionResult GetWarehouseDock(Guid warehouseId)
    {
        Warehouse? foundWarehouse = _warehouseProvider.GetById(warehouseId);
        if (foundWarehouse == null) return NotFound(new { message = $"Warehouse not found for id '{warehouseId}'" });
        List<DockItem> dockItems = _warehouseProvider.GetDockItemsByDockId(foundWarehouse.Dock.Id);
        Dock? dock = foundWarehouse.Dock;

        return Ok(new DockWithItemsResponse
        {
            Id = dock.Id,
            Capacity = Dock.CAPCITY,
            CreatedAt = dock.CreatedAt,
            UpdatedAt = dock.UpdatedAt,
            DockItems = dockItems.Select(di => new DockItemResponse()
            {
                Id = di.Id,
                ItemId = di.ItemId,
                Amount = di.Amount,
                DockId = di.DockId,
                CreatedAt = di.CreatedAt,
                UpdatedAt = di.UpdatedAt,
            }).ToList()
        });
    }
}