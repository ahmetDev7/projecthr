using DTO.Item;
using DTO.ItemGroup;
using Microsoft.AspNetCore.Mvc;
using Model;

[Route("api/[controller]")]
[ApiController]
public class ItemGroupsController : ControllerBase
{
    private ItemGroupProvider _itemGroupProvider;

    public ItemGroupsController(ItemGroupProvider itemGroupProvider)
    {
        _itemGroupProvider = itemGroupProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] ItemGroupRequest req)
    {
        ItemGroup? newItemGroup = _itemGroupProvider.Create(req);
        if (newItemGroup == null) throw new ApiFlowException("Saving new Item Group failed.");


        return Ok(new ItemGroupResponse
        {
            Id = newItemGroup.Id,
            Name = newItemGroup.Name,
            Description = newItemGroup.Description
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ItemGroupRequest req){
        ItemGroup? updatedItemGroup = _itemGroupProvider.Update(id,req);
        if(updatedItemGroup == null) return NotFound(new {message = $"Item group not found for id '{id}'"});
        
        return Ok(new {
            message = "Item group updated.", 
            updated_item_group = new ItemGroupResponse {
                Id = updatedItemGroup.Id, 
                Name = updatedItemGroup.Name, 
                Description = updatedItemGroup.Description
                }
            }
        );
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        ItemGroup? foundItemGroup = _itemGroupProvider.GetById(id);

        return (foundItemGroup == null)
            ? NotFound(new { message = $"Item group not found for id '{id}'" })
            : Ok(new ItemGroupResponse
            {
                Id = foundItemGroup.Id,
                Name = foundItemGroup.Name,
                Description = foundItemGroup.Description
            });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemGroupProvider.GetAll().Select(ig => new ItemGroupResponse
    {
        Id = ig.Id,
        Name = ig.Name,
        Description = ig.Description
    }).ToList());

    [HttpGet("{itemGroupId}/items")]
    public IActionResult ShowRelatedItems(Guid itemGroupId) =>
        Ok(_itemGroupProvider.GetRelatedItemsById(itemGroupId)
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
        }).ToList());
}