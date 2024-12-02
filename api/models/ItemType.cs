using System.ComponentModel.DataAnnotations;

public class ItemType : BaseModel
{
    public ItemType() { }
    public ItemType(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Item>? Items { get; set; } // Navigation property for Items
}