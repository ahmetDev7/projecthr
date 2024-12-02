
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
    Client? deletedClient = _clientProvider.Delete(id);

    if (deletedClient == null) BadRequest(new { message = $"Client not found for id '{id}'" });
    

    ClientResponse deletedClientResponse = new ClientResponse
    {
        Id = deletedClient.Id,
        Name = deletedClient.Name,
        CreatedAt = deletedClient.CreatedAt,
        UpdatedAt = deletedClient.UpdatedAt,
        Contact = deletedClient.ContactId.HasValue ? new ContactResponse
        {
            Name = _contactProvider.GetById(deletedClient.ContactId.Value)?.Name,
            Phone = _contactProvider.GetById(deletedClient.ContactId.Value)?.Phone,
            Email = _contactProvider.GetById(deletedClient.ContactId.Value)?.Email
        } : null,
        Address = deletedClient.AddressId.HasValue ? new AddressResponse
        {
            Street = _addressProvider.GetById(deletedClient.AddressId.Value)?.Street,
            HouseNumber = _addressProvider.GetById(deletedClient.AddressId.Value)?.HouseNumber,
            HouseNumberExtension = _addressProvider.GetById(deletedClient.AddressId.Value)?.HouseNumberExtension,
            ZipCode = _addressProvider.GetById(deletedClient.AddressId.Value)?.ZipCode,
            City = _addressProvider.GetById(deletedClient.AddressId.Value)?.City,
            Province = _addressProvider.GetById(deletedClient.AddressId.Value)?.Province,
            CountryCode = _addressProvider.GetById(deletedClient.AddressId.Value)?.CountryCode
        } : null
    };

    return Ok(new
    {
        message = "Client deleted successfully!",
        deleted_client = deletedClientResponse
    });
}


}
