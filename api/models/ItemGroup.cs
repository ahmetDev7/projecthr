using System.ComponentModel.DataAnnotations;

namespace Model;

public class ItemGroup : BaseModel
{
    public ItemGroup() : base(){}

    public ItemGroup(bool newInstance) : base(newInstance){}

    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    // Navigation property for ItemGroups
    public ICollection<Item>? ItemGroups { get; set; }
}