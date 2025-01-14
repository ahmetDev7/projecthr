using System.ComponentModel.DataAnnotations;


public class OrderItem : BaseModel
{
    public OrderItem() { }
    public OrderItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    [Required]
    public Guid? OrderId { get; set; }

    public Order? Order { get; set; }

    [Required]
    public int? Amount { get; set; }
}
