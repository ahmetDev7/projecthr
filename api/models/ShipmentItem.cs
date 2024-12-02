using System.ComponentModel.DataAnnotations;

public class ShipmentItem : BaseModel
{
    [Required]
    public Guid? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }
    
    [Required]
    public Guid? ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int? Amount { get; set; }
    
}