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

    [HttpPost]
    public IActionResult Create(SupplierRequest request)
    {
        Supplier? supplier = _supplierProvider.Create(request);
        if (supplier == null) throw new ApiFlowException("Saving new Supplier failed.");


        return Ok(new
        {
            message = "Supplier created successfully.",
            created_supplier = new SupplierResponse
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name,
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
                Created_at = supplier.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
            }
        });
    }
}