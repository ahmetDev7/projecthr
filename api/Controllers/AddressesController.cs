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
}
