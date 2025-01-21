using DTO.Item;
using DTO.ItemLine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemLinesController : ControllerBase
{
    private ItemLinesProvider _itemLinesProvider;

    public ItemLinesController(ItemLinesProvider itemLinesProvider)
    {
        _itemLinesProvider = itemLinesProvider;
    }

    [HttpPost()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager")]
    public IActionResult Create([FromBody] ItemLineRequest req)
    {
        ItemLine? newItemLine = _itemLinesProvider.Create(req);
        if (newItemLine == null) throw new ApiFlowException("Saving new Item Line failed.");

        return Ok(new
        {
            message = "Item Line created!",
            new_item_line = new ItemLineResponse
            {
                Id = newItemLine.Id,
                Name = newItemLine.Name,
                Description = newItemLine.Description,
                CreatedAt = newItemLine.CreatedAt,
                UpdatedAt = newItemLine.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager")]
    public IActionResult Update(Guid id, [FromBody] ItemLineRequest req)
    {
        ItemLine? updatedItemLine = _itemLinesProvider.Update(id, req);
        if (updatedItemLine == null) return NotFound(new { message = $"Item line not found for id '{id}'" });

        return Ok(new
        {
            message = "Item Line updated!",
            updated_item_line = new ItemLineResponse
            {
                Id = updatedItemLine.Id,
                Name = updatedItemLine.Name,
                Description = updatedItemLine.Description,
                CreatedAt = updatedItemLine.CreatedAt,
                UpdatedAt = updatedItemLine.UpdatedAt
            }
        }
        );
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult Delete(Guid id)
    {
        ItemLine? deletedItemLine = _itemLinesProvider.Delete(id);
        if (deletedItemLine == null) throw new ApiFlowException($"Item line not found for id '{id}'");

        return Ok(new
        {
            message = "Item Line deleted!",
            deleted_item_line = new ItemLineResponse
            {
                Id = deletedItemLine.Id,
                Name = deletedItemLine.Name,
                Description = deletedItemLine.Description,
                CreatedAt = deletedItemLine.CreatedAt,
                UpdatedAt = deletedItemLine.UpdatedAt
            }
        });
    }

    [HttpGet()]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
    public IActionResult ShowAll() => Ok(_itemLinesProvider.GetAll().Select(il => new ItemLineResponse
    {
        Id = il.Id,
        Name = il.Name,
        Description = il.Description,
        CreatedAt = il.CreatedAt,
        UpdatedAt = il.UpdatedAt
    }).ToList());

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
    public IActionResult ShowSingle(Guid id)
    {
        ItemLine? foundItemLine = _itemLinesProvider.GetById(id);

        return (foundItemLine == null)
            ? NotFound(new { message = $"Item Line not found for id '{id}'" })
            : Ok(new
            {
                message = "Item line found!",
                Item_Line = new ItemLineResponse
                {
                    Id = foundItemLine.Id,
                    Name = foundItemLine.Name,
                    Description = foundItemLine.Description,
                    CreatedAt = foundItemLine.CreatedAt,
                    UpdatedAt = foundItemLine.UpdatedAt
                }
            });
    }

    [HttpGet("{itemLineId}/items")]
    [Authorize(Roles = "admin,warehousemanager,inventorymanager,analyst,logistics,sales")]
    public IActionResult ShowRelatedItems(Guid itemLineId) =>
        Ok(_itemLinesProvider.GetRelatedItemsById(itemLineId)
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
            SupplierId = i.SupplierId,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        }).ToList());
}