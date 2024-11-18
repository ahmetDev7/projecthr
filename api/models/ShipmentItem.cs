using System.ComponentModel.DataAnnotations;

namespace Model;

public class ShipmentItem : BaseModel
{
    public Guid ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }
    
    [Required]
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int Amount { get; set; }
    
}