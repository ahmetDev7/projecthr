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

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SupplierRequest request)
    {
        Supplier? updatedSupplier = _supplierProvider.Update(id, request);
        if(updatedSupplier == null) return NotFound(new {message = $"Supplier not found for id '{id}'"});

        return Ok(new
        {
            message = "Supplier updated successfully.",
            updated_supplier = new SupplierResponse
            {
                Id = updatedSupplier.Id,
                Code = updatedSupplier.Code,
                Name = updatedSupplier.Name,
                Reference = updatedSupplier.Reference,
                Contact = updatedSupplier.Contact != null ? new ContactDTO
                {
                    Name = updatedSupplier.Contact.Name,
                    Email = updatedSupplier.Contact.Email,
                    Phone = updatedSupplier.Contact.Phone
                } : null,
                Address = updatedSupplier.Address != null ? new AddressDTO
                {
                    Street = updatedSupplier.Address.Street,
                    HouseNumber = updatedSupplier.Address.HouseNumber,
                    HouseNumberExtension = updatedSupplier.Address.HouseNumberExtension,
                    HouseNumberExtensionExtra = updatedSupplier.Address.HouseNumberExtensionExtra,
                    ZipCode = updatedSupplier.Address.ZipCode,
                    City = updatedSupplier.Address.City,
                    Province = updatedSupplier.Address.Province,
                    CountryCode = updatedSupplier.Address.CountryCode
                } : null
            }
        });
    }
}