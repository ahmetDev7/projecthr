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

    /*
        Create
        Update
        UpdatePartial
        Delete
        ShowSingle
        ShowAll
    */

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

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemGroupProvider.GetAll().Select(ig => new ItemGroupResponse
    {
        Id = ig.Id,
        Name = ig.Name,
        Description = ig.Description
    }).ToList());
}