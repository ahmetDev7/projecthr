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
    public IActionResult Update(Guid id, SupplierReQuestDTO request)
    {
        // Call the SupplierProvider to update the supplier
        Supplier? updatedSupplier = _supplierProvider.Update(id, request);

        if (updatedSupplier == null)
        {
            // If no supplier is found or update fails, return 400 (Bad Request) with an error message
            return BadRequest(new { error = "No Supplier found or nothing changed. Please ensure there is an update to process." });
        }

        // Return a successful response with the updated supplier details
        return Ok(new
        {
            message = "Supplier updated successfully.",
            updated_supplier = new SupplierResponseDTO
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
