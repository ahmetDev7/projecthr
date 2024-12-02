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

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ItemLineRequest req){
        ItemLine? updatedItemLine = _itemLinesProvider.Update(id,req);
        if(updatedItemLine == null) return NotFound(new {message = $"Item line not found for id '{id}'"});
        
        return Ok(new {
            message = "Item Line updated!", 
            updated_item_line = new ItemLineResponse {
                Id = updatedItemLine.Id, 
                Name = updatedItemLine.Name, 
                Description = updatedItemLine.Description,
                CreatedAt = updatedItemLine.CreatedAt,
                UpdatedAt = updatedItemLine.UpdatedAt
                }
            }
        );
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