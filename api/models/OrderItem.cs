using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model;

public class OrderItem : BaseModel
{
    public OrderItem(){}
    public OrderItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate){}

    [Required]
    public Guid? OrderId { get; set; } // bestelling 1
    public Order? Order { get; set; }

    [Required]
    public Guid? ItemId { get; set; } // id 1 (fiets)

    public Item? Item { get; set; }

    [Required]
    public int? Amount { get; set; } // 10 fietsen besteld
}
