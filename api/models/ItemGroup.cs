using System.ComponentModel.DataAnnotations;

public class ItemGroup : BaseModel
{
    public ItemGroup() { }
    public ItemGroup(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    // Navigation property for Items
    public ICollection<Item>? Items { get; set; }
}