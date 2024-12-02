using DTO.Item;
using DTO.ItemType;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemTypesController : ControllerBase
{
    private ItemTypesProvider _itemTypesProvider;

    public ItemTypesController(ItemTypesProvider itemTypesProvider)
    {
        _itemTypesProvider = itemTypesProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] ItemTypeRequest req)
    {
        ItemType? newItemType = _itemTypesProvider.Create(req);
        if (newItemType == null) throw new ApiFlowException("Saving new Item Type failed.");

        return Ok(new
        {
            message = "Item Type created!",
            new_item_type = new ItemTypeResponse
            {
                Id = newItemType.Id,
                Name = newItemType.Name,
                Description = newItemType.Description,
                CreatedAt = newItemType.CreatedAt,
                UpdatedAt = newItemType.UpdatedAt
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        ItemType? foundItemType = _itemTypesProvider.GetById(id);

        return (foundItemType == null)
            ? NotFound(new { message = $"Item Type not found for id '{id}'" })
            : Ok(new ItemTypeResponse
            {
                Id = foundItemType.Id,
                Name = foundItemType.Name,
                Description = foundItemType.Description,
                CreatedAt = foundItemType.CreatedAt,
                UpdatedAt = foundItemType.UpdatedAt
            });
    }
    
    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemTypesProvider.GetAll().Select(it => new ItemTypeResponse
    {
        Id = it.Id,
        Name = it.Name,
        Description = it.Description,
        CreatedAt = it.CreatedAt,
        UpdatedAt = it.UpdatedAt
    }).ToList());

    [HttpGet("{itemTypeId}/items")]
    public IActionResult ShowRelatedItems(Guid itemTypeId) =>
        Ok(_itemTypesProvider.GetRelatedItemsById(itemTypeId)
        .Select(i => new ItemResponse
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
            ItemLineId = i.ItemLineId,
            ItemTypeId = i.ItemTypeId,
            SupplierId = i.SupplierId
        }).ToList());
}