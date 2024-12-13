
using System.ComponentModel.DataAnnotations;

public class TransferItem : BaseModel
{
    public TransferItem() { }

    public TransferItem(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    [Required]
    public Guid? TransferId { get; set; }

    public Transfer? Transfer { get; set; }

    private int _amount = 1;

    [Required]
    public int? Amount
    {
        get { return _amount; }
        set { _amount = value.HasValue ? value.Value : 1; }
    }
}
