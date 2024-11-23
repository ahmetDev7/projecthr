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
        }});
    }
}