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
}