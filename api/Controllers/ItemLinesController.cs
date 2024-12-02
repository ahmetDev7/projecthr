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
                Description = newItemLine.Description,
                CreatedAt = newItemLine.CreatedAt,
                UpdatedAt = newItemLine.UpdatedAt
            }
        });
    }


    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemLinesProvider.GetAll().Select(il => new ItemLineResponse
    {
        Id = il.Id,
        Name = il.Name,
        Description = il.Description,
        CreatedAt = il.CreatedAt,
        UpdatedAt = il.UpdatedAt
    }).ToList());

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        ItemLine? foundItemLine = _itemLinesProvider.GetById(id);

        return (foundItemLine == null)
            ? NotFound(new { message = $"Item Line not found for id '{id}'" })
            : Ok(new ItemLineResponse
            {
                Id = foundItemLine.Id,
                Name = foundItemLine.Name,
                Description = foundItemLine.Description,
                CreatedAt = foundItemLine.CreatedAt,
                UpdatedAt = foundItemLine.UpdatedAt
            });
    }
}