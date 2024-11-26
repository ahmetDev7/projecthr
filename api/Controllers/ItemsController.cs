using Microsoft.AspNetCore.Mvc;
using DTO.Item;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ItemsProvider _itemsProvider;

    public ItemsController(ItemsProvider itemsProvider)
    {
        _itemsProvider = itemsProvider;
    }

    /*
        Create
        Update
        UpdatePartial
        Delete
        ShowSingle
        ShowAll
    */

    [HttpPost()]
    public IActionResult Create([FromBody] ItemRequest req)
    {
        Item? newItem = _itemsProvider.Create(req);
        if (newItem == null) throw new ApiFlowException("Saving new Item failed.");

        return Ok(new
        {
            message = "Item created!",
            new_item = new ItemResponse
            {
                Id = newItem.Id,
                Code = newItem.Code,
                Description = newItem.Description,
                ShortDescription = newItem.ShortDescription,
                UpcCode = newItem.UpcCode,
                ModelNumber = newItem.ModelNumber,
                CommodityCode = newItem.CommodityCode,
                UnitPurchaseQuantity = newItem.UnitPurchaseQuantity,
                UnitOrderQuantity = newItem.UnitOrderQuantity,
                PackOrderQuantity = newItem.PackOrderQuantity,
                SupplierReferenceCode = newItem.SupplierReferenceCode,
                SupplierPartNumber = newItem.SupplierPartNumber,
                ItemGroupId = newItem.ItemGroupId,
                ItemLineId = newItem.ItemLineId
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Item? foundItem = _itemsProvider.GetById(id);

        return (foundItem == null)
            ? NotFound(new { message = $"Item not found for id '{id}'" })
            : Ok(new ItemResponse
            {
                Id = foundItem.Id,
                Code = foundItem.Code,
                Description = foundItem.Description,
                ShortDescription = foundItem.ShortDescription,
                UpcCode = foundItem.UpcCode,
                ModelNumber = foundItem.ModelNumber,
                CommodityCode = foundItem.CommodityCode,
                UnitPurchaseQuantity = foundItem.UnitPurchaseQuantity,
                UnitOrderQuantity = foundItem.UnitOrderQuantity,
                PackOrderQuantity = foundItem.PackOrderQuantity,
                SupplierReferenceCode = foundItem.SupplierReferenceCode,
                SupplierPartNumber = foundItem.SupplierPartNumber,
                ItemGroupId = foundItem.ItemGroupId,
                ItemLineId = foundItem.ItemLineId
            });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemsProvider.GetAll()?.Select(i => new ItemResponse
    {
        Id = i.Id,
        Code = i.Code,
        Description = i.Description,
        ShortDescription = i.ShortDescription,
        UpcCode = i.UpcCode,
        ModelNumber = i.ModelNumber,
        CommodityCode = i.CommodityCode,
        UnitPurchaseQuantity = i.UnitPurchaseQuantity,
        UnitOrderQuantity = i.UnitOrderQuantity,
        PackOrderQuantity = i.PackOrderQuantity,
        SupplierReferenceCode = i.SupplierReferenceCode,
        SupplierPartNumber = i.SupplierPartNumber,
        ItemGroupId = i.ItemGroupId,
        ItemLineId = i.ItemLineId
    }).ToList());
}