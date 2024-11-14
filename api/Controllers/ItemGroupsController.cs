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

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id){
        ItemGroup? foundItemGroup =  _itemGroupProvider.GetById(id);
        if(foundItemGroup == null) return NotFound(new {message = $"Item group not found for id '{id}'"});

        return Ok(new ItemGroupResponse{
            Id = foundItemGroup.Id,
            Name = foundItemGroup.Name,
            Description = foundItemGroup.Description
        });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemGroupProvider.GetAll().Select(ig => new ItemGroupResponse
    {
        Id = ig.Id,
        Name = ig.Name,
        Description = ig.Description
    }).ToList());
}