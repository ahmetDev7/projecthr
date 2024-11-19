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

        return Ok(new SupplierResponseDTO
        {
            Id = supplier.Id,
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
            }
        });
    }

    [HttpGet]
    public IActionResult ShowAll() => Ok(_supplierProvider.GetAll().Select(ig => new SupplierResponseDTO
    {
        Id = ig.Id,
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
        }
    }).ToList());

    [HttpPost]
    public IActionResult Create(SupplierReQuestDTO request)
    {
        Supplier? supplier = _supplierProvider.Create(request);
        if (supplier == null) return BadRequest("Could not create supplier.");


        return Ok(new
        {
            message = "Supplier created successfully.",
            created_supplier = new SupplierResponseDTO
            {
                Id = supplier.Id,
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
                }
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SupplierReQuestDTO request)
    {
        Supplier? supplier = _supplierProvider.Update(id, request);

        if (supplier == null)
            return NotFound("If you want to update, you need to change something");

        return Ok(new
        {
            message = "Supplier updated successfully.",
            updated_supplier = new SupplierResponseDTO
            {
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
                }
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var supplier = _supplierProvider.Delete(id);

        if (supplier == null)
            return NotFound("Supplier not found.");

        return Ok("Supplier deleted successfully.");
    }
}
