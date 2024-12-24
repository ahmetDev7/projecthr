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


    [Required]
    public int? TotalOnHand
    {
        get => _totalOnHand;
        set => _totalOnHand = value == null || value < 0 ? 0 : value.Value;
    }

    [Required]
    public int? TotalExpected
    {
        get => _totalExpected;
        set => _totalExpected = value == null || value < 0 ? 0 : value.Value;
    }

    [Required]
    public int? TotalOrderd
    {
        get => _totalOrderd;
        set => _totalOrderd = value == null || value < 0 ? 0 : value.Value;
    }

    [Required]
    public int? TotalAvailable
    {
        get => _totalAvailable;
        set => _totalAvailable = value == null || value < 0 ? 0 : value.Value;
    }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }
}