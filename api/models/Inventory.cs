using System.ComponentModel.DataAnnotations;

public class Inventory : BaseModel
{
    public Inventory() { }
    public Inventory(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    public string? Description { get; set; }
    public string? ItemReference { get; set; }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }
}