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
    
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Supplier? deletedSupplier = _supplierProvider.Delete(id);
        if (deletedSupplier == null) throw new ApiFlowException($"Supplier not found for id '{id}'");
    
        return Ok(new { Message = "Supplier succesfully deleted", deleted_supplier = new SupplierResponse{
            Id = deletedSupplier.Id,
            Code = deletedSupplier.Code,
            Name = deletedSupplier.Name,
            Reference = deletedSupplier.Reference,
            Contact = new ContactDTO
            {
                Name = deletedSupplier.Contact.Name,
                Email = deletedSupplier.Contact.Email,
                Phone = deletedSupplier.Contact.Phone
            },
            Address = new AddressDTO
            {
                Street = deletedSupplier.Address.Street,
                HouseNumber = deletedSupplier.Address.HouseNumber,
                HouseNumberExtension = deletedSupplier.Address.HouseNumberExtension,
                HouseNumberExtensionExtra = deletedSupplier.Address.HouseNumberExtensionExtra,
                ZipCode = deletedSupplier.Address.ZipCode,
                City = deletedSupplier.Address.City,
                Province = deletedSupplier.Address.Province,
                CountryCode = deletedSupplier.Address.CountryCode
            },
            CreatedAt = deletedSupplier.CreatedAt,
            UpdatedAt = deletedSupplier.UpdatedAt
        });
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
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            }
        });
    }
}