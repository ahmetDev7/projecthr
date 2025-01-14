
using DTO.Address;
using DTO.Client;
using DTO.Contact;
using DTO.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly ClientsProvider _clientProvider;

    public ClientsController(ClientsProvider clientProvider, ContactProvider contactProvider, AddressProvider addressProvider)
    {
        _clientProvider = clientProvider;
    }

    [HttpPost()]
    [Authorize(Roles = "admin,warehousemanager,logistics,sales")]
    public IActionResult Create([FromBody] ClientRequest req)
    {
        Client? createClient = _clientProvider.Create(req);

        if (createClient == null) BadRequest(new { message = "Client creation failed." });

        return Ok(new
        {
            message = "Client created!",
            new_client = new ClientResponse
            {
                Id = createClient.Id,
                Name = createClient.Name,
                Contact = new ContactResponse
                {
                    Id = createClient.ContactId,
                    Name = createClient.Contact?.Name,
                    Phone = createClient.Contact?.Phone,
                    Email = createClient.Contact?.Email,
                    Function = createClient.Contact?.Function,
                    CreatedAt = createClient.Contact?.CreatedAt,
                    UpdatedAt = createClient.Contact?.UpdatedAt

                },
                Address = new AddressResponse
                {
                    Id = createClient.AddressId,
                    Street = createClient.Address?.Street,
                    HouseNumber = createClient.Address?.HouseNumber,
                    HouseNumberExtension = createClient.Address?.HouseNumberExtension,
                    HouseNumberExtensionExtra = createClient.Address?.HouseNumberExtensionExtra,
                    ZipCode = createClient.Address?.ZipCode,
                    City = createClient.Address?.City,
                    Province = createClient.Address?.Province,
                    CountryCode = createClient.Address?.CountryCode,
                    CreatedAt = createClient.Address?.CreatedAt,
                    UpdatedAt = createClient.Address?.UpdatedAt
                },
                CreatedAt = createClient.CreatedAt,
                UpdatedAt = createClient.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,warehousemanager,logistics,sales")]
    public IActionResult Update(Guid id, [FromBody] ClientRequest req)
    {
        Client? updatedClient = _clientProvider.Update(id, req);

        if (updatedClient == null) BadRequest(new { message = "Client update failed." });

        Client? foundClient = _clientProvider.GetById(updatedClient.Id);
        return Ok(new ClientResponse
        {
            Id = foundClient.Id,
            Name = foundClient.Name,
            Contact = new ContactResponse
            {
                Id = foundClient.ContactId,
                Name = foundClient.Contact?.Name,
                Function = foundClient.Contact?.Function,
                Phone = foundClient.Contact?.Phone,
                Email = foundClient.Contact?.Email,
                CreatedAt = foundClient.Contact?.CreatedAt,
                UpdatedAt = foundClient.Contact?.UpdatedAt
            },
            Address = new AddressResponse
            {
                Id = foundClient.AddressId,
                Street = foundClient.Address?.Street,
                HouseNumber = foundClient.Address?.HouseNumber,
                HouseNumberExtension = foundClient.Address?.HouseNumberExtension,
                HouseNumberExtensionExtra = foundClient.Address?.HouseNumberExtensionExtra,
                ZipCode = foundClient.Address?.ZipCode,
                City = foundClient.Address?.City,
                Province = foundClient.Address?.Province,
                CountryCode = foundClient.Address?.CountryCode,
                CreatedAt = foundClient.Address?.CreatedAt,
                UpdatedAt = foundClient.Address?.UpdatedAt
            },
            CreatedAt = foundClient.CreatedAt,
            UpdatedAt = foundClient.UpdatedAt
        });

    }


    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,analyst,logistics,sales")]
    public IActionResult ShowSingle(Guid id)
    {
        Client? foundClient = _clientProvider.GetById(id);

        if (foundClient == null) return NotFound(new { message = "Client not found." });

        return Ok(new ClientResponse
        {
            Id = foundClient.Id,
            Name = foundClient.Name,
            Contact = new ContactResponse
            {
                Id = foundClient.ContactId,
                Name = foundClient.Contact?.Name,
                Function = foundClient.Contact?.Function,
                Phone = foundClient.Contact?.Phone,
                Email = foundClient.Contact?.Email,
                CreatedAt = foundClient.Contact?.CreatedAt,
                UpdatedAt = foundClient.Contact?.UpdatedAt

            },
            Address = new AddressResponse
            {
                Id = foundClient.AddressId,
                Street = foundClient.Address?.Street,
                HouseNumber = foundClient.Address?.HouseNumber,
                HouseNumberExtension = foundClient.Address?.HouseNumberExtension,
                HouseNumberExtensionExtra = foundClient.Address?.HouseNumberExtensionExtra,
                ZipCode = foundClient.Address?.ZipCode,
                City = foundClient.Address?.City,
                Province = foundClient.Address?.Province,
                CountryCode = foundClient.Address?.CountryCode,
                CreatedAt = foundClient.Address?.CreatedAt,
                UpdatedAt = foundClient.Address?.UpdatedAt
            },
            CreatedAt = foundClient.CreatedAt,
            UpdatedAt = foundClient.UpdatedAt
        });
    }


    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,floormanager,analyst,logistics,sales")]
    public IActionResult ShowAll()
    {
        List<Client>? clients = _clientProvider.GetAll();

        List<ClientResponse>? clientResponses = clients?.Select(c => new ClientResponse
        {
            Id = c.Id,
            Name = c.Name,
            Contact = new ContactResponse
            {
                Id = c.ContactId,
                Name = c.Contact?.Name,
                Function = c.Contact?.Function,
                Phone = c.Contact?.Phone,
                Email = c.Contact?.Email,
                CreatedAt = c.Contact?.CreatedAt,
                UpdatedAt = c.Contact?.UpdatedAt
            },
            Address = new AddressResponse
            {
                Id = c.AddressId,
                Street = c.Address?.Street,
                HouseNumber = c.Address?.HouseNumber,
                HouseNumberExtension = c.Address?.HouseNumberExtension,
                HouseNumberExtensionExtra = c.Address?.HouseNumberExtensionExtra,
                ZipCode = c.Address?.ZipCode,
                City = c.Address?.City,
                Province = c.Address?.Province,
                CountryCode = c.Address?.CountryCode,
                CreatedAt = c.Address?.CreatedAt,
                UpdatedAt = c.Address?.UpdatedAt
            },
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return Ok(clientResponses);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult Delete(Guid id)
    {
        Client? deletedClient = _clientProvider.Delete(id);

        if (deletedClient == null) return NotFound(new { message = "Client not found." });

        return Ok(new
        {
            Message = "Client deleted",
            new_client = new ClientResponse
            {

                Id = deletedClient.Id,
                Name = deletedClient.Name,
                Contact = new ContactResponse
                {
                    Id = deletedClient.ContactId,
                    Name = deletedClient.Contact?.Name,
                    Function = deletedClient.Contact?.Function,
                    Phone = deletedClient.Contact?.Phone,
                    Email = deletedClient.Contact?.Email,
                    CreatedAt = deletedClient.Contact?.CreatedAt,
                    UpdatedAt = deletedClient.Contact?.UpdatedAt
                },
                Address = new AddressResponse
                {
                    Id = deletedClient.AddressId,
                    Street = deletedClient.Address?.Street,
                    HouseNumber = deletedClient.Address?.HouseNumber,
                    HouseNumberExtension = deletedClient.Address?.HouseNumberExtension,
                    ZipCode = deletedClient.Address?.ZipCode,
                    City = deletedClient.Address?.City,
                    Province = deletedClient.Address?.Province,
                    CountryCode = deletedClient.Address?.CountryCode,
                    CreatedAt = deletedClient.Address?.CreatedAt,
                    UpdatedAt = deletedClient.Address?.UpdatedAt
                },
                CreatedAt = deletedClient.CreatedAt,
                UpdatedAt = deletedClient.UpdatedAt
            }
        });
    }

    [HttpGet("{clientId}/orders")]
    public IActionResult ShowRelatedOrders(Guid clientId) =>
        Ok(_clientProvider.GetRelatedOrdersById(clientId)
        .Select(o => new OrderResponse
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            RequestDate = o.RequestDate,
            Reference = o.Reference,
            ReferenceExtra = o.ReferenceExtra,
            OrderStatus = o.OrderStatus,
            Notes = o.Notes,
            PickingNotes = o.PickingNotes,
            TotalAmount = o.TotalAmount,
            TotalDiscount = o.TotalDiscount,
            TotalTax = o.TotalTax,
            TotalSurcharge = o.TotalSurcharge,
            WarehouseId = o.WarehouseId,
            ShipToClientId = o.ShipToClientId,
            BillToClientId = o.BillToClientId,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
            Items = o.OrderItems?.Select(oi => new OrderItemRequest
            {
                ItemId = oi.ItemId,
                Amount = oi.Amount
            }).ToList()
        }).ToList());
}
