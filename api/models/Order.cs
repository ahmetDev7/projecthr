using System.ComponentModel.DataAnnotations;

public class Order : BaseModel
{
    public Order() { }
    public Order(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? Reference { get; set; }
    public string? ReferenceExtra { get; set; }
    [Required]
    public string? OrderStatus { get; set; }
    public string? Notes { get; set; }
    public string? PickingNotes { get; set; }
    [Required]
    public Guid? WarehouseId { get; set; }

    public Warehouse? Warehouse { get; set; }
    [Required]
    public ICollection<OrderItem>? OrderItems { get; set; }
    //TODO:
    //  shipment_id uuid [ref: > shipments.id]
    //TODO..
    //  ship_to_client uuid [ref: > clients.id]
    //[Required]
    //bill_to_client uuid [ref: > clients.id]

}