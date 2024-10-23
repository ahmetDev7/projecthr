using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemsController: ControllerBase {

    readonly ItemsProvider _itemsProvider;
    public ItemsController(ItemsProvider itemsProvider){
        _itemsProvider = itemsProvider;
    }
    
    public IActionResult GetAll(){
        List<Item>? items = _itemsProvider.GetAll();
        if(items == null) return NotFound(new {message = "No items found."});

        return Ok(items);
    }
}