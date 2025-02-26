using DTO.Item;
using DTO.ItemGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "admin,warehousemanager,inventorymanager")]
    public IActionResult Create([FromBody] ItemGroupRequest req)
    {
        ItemGroup? newItemGroup = _itemGroupProvider.Create(req);
        if (newItemGroup == null) throw new ApiFlowException("Saving new Item Group failed.");


        return Ok(new
        {
            message = "Item group created!",
            ItemGroup = new ItemGroupResponse
            {
                Id = newItemGroup.Id,
                Name = newItemGroup.Name,
                Description = newItemGroup.Description,
                CreatedAt = newItemGroup.CreatedAt,
                UpdatedAt = newItemGroup.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager")]
    public IActionResult Update(Guid id, [FromBody] ItemGroupRequest req)
    {
        ItemGroup? updatedItemGroup = _itemGroupProvider.Update(id, req);
        if (updatedItemGroup == null) return NotFound(new { message = $"Item group not found for id '{id}'" });

        return Ok(new
        {
            message = "Item group updated.",
            updated_item_group = new ItemGroupResponse
            {
                Id = updatedItemGroup.Id,
                Name = updatedItemGroup.Name,
                Description = updatedItemGroup.Description,
                CreatedAt = updatedItemGroup.CreatedAt,
                UpdatedAt = updatedItemGroup.UpdatedAt
            }
        }
        );
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult Delete(Guid id)
    {
        ItemGroup? deletedItemGroup = _itemGroupProvider.Delete(id);
        if (deletedItemGroup == null) throw new ApiFlowException($"Item group not found for id '{id}'");

        return Ok(new
        {
            message = "Item group deleted!",
            deleted_item_group = new ItemGroupResponse
            {
                Id = deletedItemGroup.Id,
                Name = deletedItemGroup.Name,
                Description = deletedItemGroup.Description,
                CreatedAt = deletedItemGroup.CreatedAt,
                UpdatedAt = deletedItemGroup.UpdatedAt
            }
        });

    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
    public IActionResult ShowSingle(Guid id)
    {
        ItemGroup? foundItemGroup = _itemGroupProvider.GetById(id);

        return (foundItemGroup == null)
            ? NotFound(new { message = $"Item group not found for id '{id}'" })
            : Ok(new
            {
                message = "Item group found!",
                ItemGroup = new ItemGroupResponse
                {
                    Id = foundItemGroup.Id,
                    Name = foundItemGroup.Name,
                    Description = foundItemGroup.Description,
                    CreatedAt = foundItemGroup.CreatedAt,
                    UpdatedAt = foundItemGroup.UpdatedAt
                }
            });
    }

    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
    public IActionResult ShowAll() => Ok(_itemGroupProvider.GetAll().Select(ig => new ItemGroupResponse
    {
        Id = ig.Id,
        Name = ig.Name,
        Description = ig.Description
    }).ToList());

    [HttpGet("{itemGroupId}/items")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
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
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt,
        }).ToList());
}