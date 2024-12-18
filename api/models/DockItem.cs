using System.ComponentModel.DataAnnotations;

public class DockItem : BaseModel
{
    public DockItem() { }

    public DockItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    [Required]
    public Guid? DockId { get; set; }

    public Dock? Dock { get; set; }

    private int? _amount = 0;

    [Required]
    public int? Amount
    {
        get => _amount;
        set => _amount = value.HasValue && value > 0 ? value : 0;
    }
}
