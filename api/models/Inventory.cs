using System.ComponentModel.DataAnnotations;

public class Inventory : BaseModel
{
    public Inventory() { }
    public Inventory(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    public string? Description { get; set; }
    public string? ItemReference { get; set; }

    private int _totalOnHand = 0;
    private int _totalExpected = 0;
    private int _totalOrderd = 0;
    private int _totalAvailable = 0;
    private int _totalAllocated = 0;


    [Required]
    public int TotalOnHand
    {
        get => _totalOnHand;
        set => _totalOnHand = value < 0 ? 0 : value;
    }

    [Required]
    public int TotalExpected
    {
        get => _totalExpected;
        set => _totalExpected = value < 0 ? 0 : value;
    }

    [Required]
    public int TotalOrderd
    {
        get => _totalOrderd;
        set => _totalOrderd = value < 0 ? 0 : value;
    }

    [Required]
    public int TotalAvailable
    {
        get => _totalAvailable;
        set => _totalAvailable = value < 0 ? 0 : value;
    }

    [Required]
    public int TotalAllocated
    {
        get => _totalAllocated;
        set => _totalAllocated = value < 0 ? 0 : value;
    }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    public ICollection<InventoryLocation>? InventoryLocations { get; set; }
}