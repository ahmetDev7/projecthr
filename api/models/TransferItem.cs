
using System.ComponentModel.DataAnnotations;

public class TransferItem : BaseModel
{
    public TransferItem() { }

    public TransferItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    [Required]
    public int? Amount { get; set; }
}
