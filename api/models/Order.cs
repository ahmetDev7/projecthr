using System.ComponentModel.DataAnnotations;


namespace Model;

public class Order : BaseModel
{
    public Order(){}
    public Order(bool newInstance = false, bool isUpdate = false ) : base(newInstance, isUpdate){}
    [Required]
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? Reference{ get; set; }
    public string? ReferenceExtra { get; set; }
    [Required]
    public string? OrderStatus  { get; set; }
    public string? Notes  { get; set; }
    public string? ShipToClient  { get; set; }
    public string? PickingNotes  { get; set; }
    //TODO..
    //  ship_to_client uuid [ref: > clients.id]
    //[Required]
    //bill_to_client uuid [ref: > clients.id]
    public float? TotalAmount  { get; set; }
    public float? TotalDiscount  { get; set; }
    public float? TotalTax  { get; set; }
    public float? TotalSurcharge  { get; set; }
    [Required]
    public Guid? WarehouseId  { get; set; }
    //TODO:
    //  shipment_id uuid [ref: > shipments.id]


}
