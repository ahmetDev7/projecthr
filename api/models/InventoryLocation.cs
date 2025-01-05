using System.ComponentModel.DataAnnotations;

public class InventoryLocation : BaseModel
{
    public InventoryLocation() { }

    public InventoryLocation(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? InventoryId { get; set; }

    public Inventory? Inventory { get; set; }

    [Required]
    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    private int _onHandAmount = 0;

    [Required]
    public int OnHandAmount
    {
        get => _onHandAmount;
        set => _onHandAmount = value > 0 ? value : 0;
    }
}
