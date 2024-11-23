using Microsoft.AspNetCore.Mvc;
using DTO.Supplier;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierProvider _supplierProvider;

    public SuppliersController(SupplierProvider supplierProvider)
    {
        _supplierProvider = supplierProvider;
    }

    [HttpGet]
    public IActionResult ShowAll() => Ok(_supplierProvider.GetAll().Select(ig => new SupplierResponse
    {
        Id = ig.Id,
        Code = ig.Code,
        Name = ig.Name,
        Reference = ig.Reference,
        Contact = new ContactDTO
        {
            Name = ig.Contact.Name,
            Email = ig.Contact.Email,
            Phone = ig.Contact.Phone
        },
        Address = new AddressDTO
        {
            Street = ig.Address.Street,
            HouseNumber = ig.Address.HouseNumber,
            HouseNumberExtension = ig.Address.HouseNumberExtension,
            HouseNumberExtensionExtra = ig.Address.HouseNumberExtensionExtra,
            ZipCode = ig.Address.ZipCode,
            City = ig.Address.City,
            Province = ig.Address.Province,
            CountryCode = ig.Address.CountryCode,
        },
        CreatedAt = ig.CreatedAt,
        UpdatedAt = ig.UpdatedAt
        
    }).ToList());

}