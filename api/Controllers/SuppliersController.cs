using Microsoft.AspNetCore.Mvc;
using DTO.Supplier;
using DTO.Item;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierProvider _supplierProvider;

    public SuppliersController(SupplierProvider supplierProvider)
    {
        _supplierProvider = supplierProvider;
    }

    [HttpGet("{id}/items")]
    public IActionResult GetItemsSupplier(Guid id)
    {
        var items = _supplierProvider.GetItemsBySupplierId(id);
        if (items == null) throw new ApiFlowException("this supplier does not have any items yet");

        var itemsSupplier = items.Select(item => new ItemResponse
        {
            Id = item.Id,
            Code = item.Code,
            Description = item.Description,
            ShortDescription = item.ShortDescription,
            UpcCode = item.UpcCode,
            ModelNumber = item.ModelNumber,
            CommodityCode = item.CommodityCode,
            UnitPurchaseQuantity = item.UnitPurchaseQuantity,
            UnitOrderQuantity = item.UnitOrderQuantity,
            PackOrderQuantity = item.PackOrderQuantity,
            SupplierReferenceCode = item.SupplierReferenceCode,
            SupplierPartNumber = item.SupplierPartNumber,
            ItemGroupId = item.ItemGroupId,
            SupplierId = item.SupplierId,
        }).ToList();

        return Ok(itemsSupplier);
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