using Microsoft.AspNetCore.Mvc;
using DTO.Address;



[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly AddressProvider _addressProvider;

    public AddressesController(AddressProvider addressProvider)
    {
        _addressProvider = addressProvider;
    }

    [HttpPost]
    public IActionResult Create([FromBody] AddressRequest req)
    {
        Address? newAdress = _addressProvider.Create(req);

        if (newAdress == null) throw new ApiFlowException("Saving new Address failed.");

        return Ok(new 
        {
            message = "Address created",
            new_address = new AddressResponse
            {
                Id = newAdress.Id,
                Street = newAdress.Street,
                HouseNumber = newAdress.HouseNumber,
                HouseNumberExtension = newAdress.HouseNumberExtension,
                HouseNumberExtensionExtra = newAdress.HouseNumberExtensionExtra,
                ZipCode = newAdress.ZipCode,
                City = newAdress.City,
                Province = newAdress.Province,
                CountryCode = newAdress.CountryCode
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Address? foundAddress = _addressProvider.GetById(id);

        if (foundAddress == null) throw new ApiFlowException($"Address not found for id '{id}'");

        return Ok(new 
        {
            message = "Address found",
            new_address = new AddressResponse
            {
                Id = foundAddress.Id,
                Street = foundAddress.Street,
                HouseNumber = foundAddress.HouseNumber,
                HouseNumberExtension = foundAddress.HouseNumberExtension,
                HouseNumberExtensionExtra = foundAddress.HouseNumberExtensionExtra,
                ZipCode = foundAddress.ZipCode,
                City = foundAddress.City,
                Province = foundAddress.Province,
                CountryCode = foundAddress.CountryCode
            }
        });
    }
}
