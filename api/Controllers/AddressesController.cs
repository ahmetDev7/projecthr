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
        Address? newAddress = _addressProvider.Create(req);

        if (newAddress == null) throw new ApiFlowException("Saving new Address failed.");

        return Ok(new
        {
            message = "Address created",
            new_address = new AddressResponse
            {
                Id = newAddress.Id,
                Street = newAddress.Street,
                HouseNumber = newAddress.HouseNumber,
                HouseNumberExtension = newAddress.HouseNumberExtension,
                HouseNumberExtensionExtra = newAddress.HouseNumberExtensionExtra,
                ZipCode = newAddress.ZipCode,
                City = newAddress.City,
                Province = newAddress.Province,
                CountryCode = newAddress.CountryCode,
                CreatedAt = newAddress.CreatedAt,
                UpdatedAt = newAddress.UpdatedAt

            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] AddressRequest req)
    {
        Address? updateAddress = _addressProvider.Update(id, req);

        if (updateAddress == null) throw new ApiFlowException($"Address not found for id '{id}'");

        return Ok(new
        {
            message = "Address updated",
            new_address = new AddressResponse
            {
                Id = updateAddress.Id,
                Street = updateAddress.Street,
                HouseNumber = updateAddress.HouseNumber,
                HouseNumberExtension = updateAddress.HouseNumberExtension,
                HouseNumberExtensionExtra = updateAddress.HouseNumberExtensionExtra,
                ZipCode = updateAddress.ZipCode,
                City = updateAddress.City,
                Province = updateAddress.Province,
                CountryCode = updateAddress.CountryCode,
                CreatedAt = updateAddress.CreatedAt,
                UpdatedAt = updateAddress.UpdatedAt
            }
        });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_addressProvider.GetAll().Select(a => new AddressResponse
    {
        Id = a.Id,
        Street = a.Street,
        HouseNumber = a.HouseNumber,
        HouseNumberExtension = a.HouseNumberExtension,
        HouseNumberExtensionExtra = a.HouseNumberExtensionExtra,
        ZipCode = a.ZipCode,
        City = a.City,
        Province = a.Province,
        CountryCode = a.CountryCode,
        CreatedAt = a.CreatedAt,
        UpdatedAt = a.UpdatedAt
    }));

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Address? foundAddress = _addressProvider.GetById(id);
        if (foundAddress == null) throw new ApiFlowException($"Address not found for id '{id}'", StatusCodes.Status404NotFound);

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
                CountryCode = foundAddress.CountryCode,
                CreatedAt = foundAddress.CreatedAt,
                UpdatedAt = foundAddress.UpdatedAt
            }
        });
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Address? deletedAddress = _addressProvider.Delete(id);
        if (deletedAddress == null) throw new ApiFlowException($"Address not found for id '{id}'", StatusCodes.Status404NotFound);

        return Ok(new
        {
            message = "Address Deleted",
            new_address = new AddressResponse
            {
                Id = deletedAddress.Id,
                Street = deletedAddress.Street,
                HouseNumber = deletedAddress.HouseNumber,
                HouseNumberExtension = deletedAddress.HouseNumberExtension,
                HouseNumberExtensionExtra = deletedAddress.HouseNumberExtensionExtra,
                ZipCode = deletedAddress.ZipCode,
                City = deletedAddress.City,
                Province = deletedAddress.Province,
                CountryCode = deletedAddress.CountryCode,
                CreatedAt = deletedAddress.CreatedAt,
                UpdatedAt = deletedAddress.UpdatedAt
            }
        });
    }
}
