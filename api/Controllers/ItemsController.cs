using DTOs;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ItemsProvider _itemsProvider;

    public ItemsController(ItemsProvider itemsProvider)
    {
        _itemsProvider = itemsProvider;
    }

    [HttpPost()]
    public IActionResult CreateItem([FromBody] ItemDTO req)
    {
        try
        {
            Item? newItem = _itemsProvider.Create<ItemDTO>(req);
            if (newItem == null) throw new ApiFlowException("Saving new Item failed.");


            return Ok(new { message = "Item created!", new_item = req });
        }
        catch (ApiFlowException apiFlowException)
        {
            return Problem(apiFlowException.Message, statusCode: 500);
        }
        catch (Exception)
        {
            return Problem("An error occurred while creating an item. Please try again.", statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    public IActionResult SingleItem(Guid id)
    {
        Item? foundItem = _itemsProvider.GetById(id);
        if (foundItem == null) return NotFound(new { message = $"Item not found for id '{id}'" });
        return Ok(foundItem);
    }
}