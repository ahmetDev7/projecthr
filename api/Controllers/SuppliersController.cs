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
    
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        Supplier? supplier = _supplierProvider.GetById(id);

        if (supplier == null) return NotFound("Supplier not found.");

        return Ok(new SupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Code = supplier.Code,
            Reference = supplier.Reference,
            Contact = new ContactDTO
            {
                Name = supplier.Contact.Name,
                Email = supplier.Contact.Email,
                Phone = supplier.Contact.Phone
            },
            Address = new AddressDTO
            {
                Street = supplier.Address.Street,
                HouseNumber = supplier.Address.HouseNumber,
                HouseNumberExtension = supplier.Address.HouseNumberExtension,
                HouseNumberExtensionExtra = supplier.Address.HouseNumberExtensionExtra,
                ZipCode = supplier.Address.ZipCode,
                City = supplier.Address.City,
                Province = supplier.Address.Province,
                CountryCode = supplier.Address.CountryCode,
            },
            Created_at = supplier.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
            Updated_at = supplier.UpdatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}