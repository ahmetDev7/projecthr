
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
                Id = createdClient.AddressId,
                Name = createdClient.Contact?.Name,
                Phone = createdClient.Contact?.Phone,
                Email = createdClient.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = createdClient.ContactId,
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
                Id = c.AddressId,
                Name = c.Contact?.Name,
                Phone = c.Contact?.Phone,
                Email = c.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = c.ContactId,
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
}
