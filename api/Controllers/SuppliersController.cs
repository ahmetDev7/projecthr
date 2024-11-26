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


   [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        Supplier? supplier = _supplierProvider.GetById(id);

        if (supplier == null) return NotFound(new { message = $"Supplier not found for id '{id}'" });

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
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SupplierRequest request)
    {
        Supplier? updatedSupplier = _supplierProvider.Update(id, request);
        if (updatedSupplier == null) return NotFound(new { message = $"Supplier not found for id '{id}'" });

        return Ok(new
        {
            message = "Supplier updated successfully.",
            updated_supplier = new SupplierResponse
            {
                Id = updatedSupplier.Id,
                Code = updatedSupplier.Code,
                Name = updatedSupplier.Name,
                Reference = updatedSupplier.Reference,
                Contact = new ContactDTO
                {
                    Name = updatedSupplier.Contact.Name,
                    Email = updatedSupplier.Contact.Email,
                    Phone = updatedSupplier.Contact.Phone
                },
                Address = new AddressDTO
                {
                    Street = updatedSupplier.Address.Street,
                    HouseNumber = updatedSupplier.Address.HouseNumber,
                    HouseNumberExtension = updatedSupplier.Address.HouseNumberExtension,
                    HouseNumberExtensionExtra = updatedSupplier.Address.HouseNumberExtensionExtra,
                    ZipCode = updatedSupplier.Address.ZipCode,
                    City = updatedSupplier.Address.City,
                    Province = updatedSupplier.Address.Province,
                    CountryCode = updatedSupplier.Address.CountryCode
                },
                CreatedAt = updatedSupplier.CreatedAt,
                UpdatedAt = updatedSupplier.UpdatedAt
            }
        });
    }
}