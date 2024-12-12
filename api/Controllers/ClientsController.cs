
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

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Client? foundclient = _clientProvider.Delete(id);

        if (foundclient == null) return NotFound(new { message = "Client not found." });

        return Ok(new ClientResponse
        {
            Id = foundclient.Id,
            Name = foundclient.Name,
            Contact = new ContactResponse
            {
                Id = foundclient.ContactId,
                Name = foundclient.Contact?.Name,
                Phone = foundclient.Contact?.Phone,
                Email = foundclient.Contact?.Email
            },
            Address = new AddressResponse
            {
                Id = foundclient.AddressId,
                Street = foundclient.Address?.Street,
                HouseNumber = foundclient.Address?.HouseNumber,
                HouseNumberExtension = foundclient.Address?.HouseNumberExtension,
                ZipCode = foundclient.Address?.ZipCode,
                City = foundclient.Address?.City,
                Province = foundclient.Address?.Province,
                CountryCode = foundclient.Address?.CountryCode
            },
            CreatedAt = foundclient.CreatedAt,
            UpdatedAt = foundclient.UpdatedAt
        });
    }


}
