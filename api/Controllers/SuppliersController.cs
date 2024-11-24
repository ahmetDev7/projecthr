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

    [HttpGet("{Reference}/items")]
    public IActionResult GetItemsSupplier(string Reference)
    {
        var items = _supplierProvider.GetItemsByReferenceCode(Reference);
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
            ItemGroupId = item.ItemGroupId
        }).ToList();

        return Ok(itemsSupplier);
    }
}