using System.ComponentModel.DataAnnotations;

public class ItemLine : BaseModel
{
    public ItemLine() { }
    public ItemLine(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Item>? Items { get; set; }     // Navigation property for Items
}