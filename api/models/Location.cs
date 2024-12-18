using System.ComponentModel.DataAnnotations;

public class Location : BaseModel
{
    public Location() { }
    public Location(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Row { get; set; }
    [Required]
    public string? Rack { get; set; }
    [Required]
    public string? Shelf { get; set; }
    [Required]
    public Guid? WarehouseId { get; set; } // Foreign Key Relationship
    public Warehouse? Warehouse { get; set; }

    public ICollection<Transfer>? TransfersTo { get; set; }
    public ICollection<Transfer>? TransfersFrom { get; set; }

    private int? _onHand = 0;
    public int? OnHand
    {
        get => _onHand;
        set => _onHand = value.HasValue && value > 0 ? value : 0;
    }

    public Guid? InventoryId {get; set;}

    public Inventory? Inventory{ get; set; }
}