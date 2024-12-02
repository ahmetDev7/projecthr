using DTO.ItemLine;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemLineController : ControllerBase
{
    private ItemLineProvider _itemLineProvider;

    public ItemLineController(ItemLineProvider itemLineProvider)
    {
        _itemLineProvider = itemLineProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] ItemLineRequest req)
    {
        ItemLine? newItemLine = _itemLineProvider.Create(req);
        if (newItemLine == null) throw new ApiFlowException("Saving new Item Line failed.");


        return Ok(new ItemLineResponse
        {
            Id = newItemLine.Id,
            Name = newItemLine.Name,
            Description = newItemLine.Description
        });
    }
}