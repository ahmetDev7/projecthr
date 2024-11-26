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
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
            
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
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            }
        });
    }
}