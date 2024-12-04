
using DTO.Address;
using DTO.Client;
using DTO.Contact;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly ClientsProvider _clientProvider;
    private readonly ContactProvider _contactProvider;
    private readonly AddressProvider _addressProvider;
    public ClientsController(ClientsProvider clientProvider, ContactProvider contactProvider, AddressProvider addressProvider)
    {
        _clientProvider = clientProvider;
        _contactProvider = contactProvider;
        _addressProvider = addressProvider;
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
                    Name = newClient.Contact?.Name,
                    Phone = newClient.Contact?.Phone,
                    Email = newClient.Contact?.Email
                },
                Address = new AddressResponse
                {
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

        ClientResponse updatedClientResponse = new ClientResponse
        {
            Id = updatedClient.Id,
            Name = updatedClient.Name,
            CreatedAt = updatedClient.CreatedAt,
            UpdatedAt = updatedClient.UpdatedAt,

            Contact = updatedClient.ContactId.HasValue ? new ContactResponse
            {
                Name = _contactProvider.GetById(updatedClient.ContactId.Value)?.Name,
                Phone = _contactProvider.GetById(updatedClient.ContactId.Value)?.Phone,
                Email = _contactProvider.GetById(updatedClient.ContactId.Value)?.Email
            } : null,

            Address = updatedClient.AddressId.HasValue ? new AddressResponse
            {
                Street = _addressProvider.GetById(updatedClient.AddressId.Value)?.Street,
                HouseNumber = _addressProvider.GetById(updatedClient.AddressId.Value)?.HouseNumber,
                HouseNumberExtension = _addressProvider.GetById(updatedClient.AddressId.Value)?.HouseNumberExtension,
                ZipCode = _addressProvider.GetById(updatedClient.AddressId.Value)?.ZipCode,
                City = _addressProvider.GetById(updatedClient.AddressId.Value)?.City,
                Province = _addressProvider.GetById(updatedClient.AddressId.Value)?.Province,
                CountryCode = _addressProvider.GetById(updatedClient.AddressId.Value)?.CountryCode
            } : null
        };

        return Ok(new
        {
            message = "Client updated successfully!",
            updated_client = updatedClientResponse
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
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            Contact = c.ContactId.HasValue ? new ContactResponse
            {
                Name = _contactProvider.GetById(c.ContactId.Value)?.Name,
                Phone = _contactProvider.GetById(c.ContactId.Value)?.Phone,
                Email = _contactProvider.GetById(c.ContactId.Value)?.Email
            } : null,
            Address = c.AddressId.HasValue ? new AddressResponse
            {
                Street = _addressProvider.GetById(c.AddressId.Value)?.Street,
                HouseNumber = _addressProvider.GetById(c.AddressId.Value)?.HouseNumber,
                HouseNumberExtension = _addressProvider.GetById(c.AddressId.Value)?.HouseNumberExtension,
                ZipCode = _addressProvider.GetById(c.AddressId.Value)?.ZipCode,
                City = _addressProvider.GetById(c.AddressId.Value)?.City,
                Province = _addressProvider.GetById(c.AddressId.Value)?.Province,
                CountryCode = _addressProvider.GetById(c.AddressId.Value)?.CountryCode
            } : null
        }).ToList();

        return Ok(clientResponses);
    }
}

public class UpdateRequest
{
}