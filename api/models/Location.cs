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
}