using System.ComponentModel.DataAnnotations;

public class Transfer : BaseModel
{
    public Transfer() { }

    public Transfer(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    public string? Reference { get; set; }

    [Required]
    public Guid? TransferFromId { get; set; }
    public Location? TransferFrom {get; set;}

    [Required]
    public Guid? TransferToId { get; set; }
    public Location? TransferTo {get; set;}

    [Required]
    public TransferStatus? TransferStatus { get; set; }

    public ICollection<TransferItem>? TransferItems { get; set; }

    public void SetTransferStatus(string? strTransferStatus) =>
    this.TransferStatus = strTransferStatus == null
        ? global::TransferStatus.Pending  // on strTransferStatus null set to default (Pending)
        : EnumUtil.ParseOrIgnore<TransferStatus>(strTransferStatus);

}
