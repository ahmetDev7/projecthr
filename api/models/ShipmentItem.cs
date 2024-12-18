using System.ComponentModel.DataAnnotations;

public class ShipmentItem : BaseModel
{
    public ShipmentItem()
    {
    }
    public ShipmentItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate)
    {
    }

    [Required]
    public Guid? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }

    [Required]
    public Guid? ItemId { get; set; }
    public Item? Item { get; set; }

    [Required]
    public int? Amount { get; set; }

}