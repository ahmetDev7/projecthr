
using DTO.Address;
using DTO.Client;
using DTO.Contact;
using DTO.Order;
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
    public IActionResult Create([FromBody] ClientRequest req)
    {
        Client? newClient = _clientProvider.Create(req);

        if (newClient == null) BadRequest(new { message = "Client creation failed." });

        return Ok(new
        {
            message = "Client created!",
            new_client = new ClientResponse
            {
                Id = newClient.Id,
                Name = newClient.Name,
                Contact = new ContactResponse
                {
                    Id = newClient.ContactId,
                    Name = newClient.Contact?.Name,
                    Phone = newClient.Contact?.Phone,
                    Email = newClient.Contact?.Email
                },
                Address = new AddressResponse
                {
                    Id = newClient.AddressId,
                    Street = newClient.Address?.Street,
                    HouseNumber = newClient.Address?.HouseNumber,
                    HouseNumberExtension = newClient.Address?.HouseNumberExtension,
                    ZipCode = newClient.Address?.ZipCode,
                    City = newClient.Address?.City,
                    Province = newClient.Address?.Province,
                    CountryCode = newClient.Address?.CountryCode
                },
                CreatedAt = newClient.CreatedAt,
                UpdatedAt = newClient.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
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
                Phone = foundClient.Contact?.Phone,
                Email = foundClient.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = foundClient.AddressId,
                Street = foundClient.Address?.Street,
                HouseNumber = foundClient.Address?.HouseNumber,
                HouseNumberExtension = foundClient.Address?.HouseNumberExtension,
                ZipCode = foundClient.Address?.ZipCode,
                City = foundClient.Address?.City,
                Province = foundClient.Address?.Province,
                CountryCode = foundClient.Address?.CountryCode
            },
            CreatedAt = foundClient.CreatedAt,
            UpdatedAt = foundClient.UpdatedAt
        });

    }

    
    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Client? createdClient = _clientProvider.GetById(id);

        if (createdClient == null) return NotFound(new { message = "Client not found." });

        return Ok(new ClientResponse
        {
            Id = createdClient.Id,
            Name = createdClient.Name,
            Contact = new ContactResponse
            {
                Id = createdClient.ContactId,
                Name = createdClient.Contact?.Name,
                Phone = createdClient.Contact?.Phone,
                Email = createdClient.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = createdClient.AddressId,
                Street = createdClient.Address?.Street,
                HouseNumber = createdClient.Address?.HouseNumber,
                HouseNumberExtension = createdClient.Address?.HouseNumberExtension,
                ZipCode = createdClient.Address?.ZipCode,
                City = createdClient.Address?.City,
                Province = createdClient.Address?.Province,
                CountryCode = createdClient.Address?.CountryCode
            },
            CreatedAt = createdClient.CreatedAt,
            UpdatedAt = createdClient.UpdatedAt
        });
    }


    [HttpGet]
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
                Phone = c.Contact?.Phone,
                Email = c.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = c.AddressId,
                Street = c.Address?.Street,
                HouseNumber = c.Address?.HouseNumber,
                HouseNumberExtension = c.Address?.HouseNumberExtension,
                ZipCode = c.Address?.ZipCode,
                City = c.Address?.City,
                Province = c.Address?.Province,
                CountryCode = c.Address?.CountryCode
            },
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return Ok(clientResponses);
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
