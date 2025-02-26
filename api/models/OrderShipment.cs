using System.ComponentModel.DataAnnotations;

public class OrderShipment : BaseModel
{
    public OrderShipment() { }
    public OrderShipment(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    public Shipment? Shipment { get; set; }

    public Order? Order { get; set; }

    [Required]
    public Guid? OrderId { get; set; }

    [Required]
    public Guid? ShipmentId { get; set; }

}
