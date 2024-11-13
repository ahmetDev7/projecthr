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


        return Ok(new ItemGroupResponse{
            Id = newItemGroup.Id,
            Name = newItemGroup.Name,
            Description = newItemGroup.Description
        });
    }
}