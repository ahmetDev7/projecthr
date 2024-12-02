using DTO.ItemLine;
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
                Description = newItemLine.Description
            }
        });
    }
}