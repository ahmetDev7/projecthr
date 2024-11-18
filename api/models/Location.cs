using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Model;

public class Location : BaseModel
{
    public Location(){}

    public Location(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate){}
    
    [Required]
    public string? Row { get; set; }
    [Required]
    public string? Rack { get; set; }
    [Required]
    public string? Shelf { get; set; }
    // Foreign Key Relationship
    [Required]
    public Guid? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}
