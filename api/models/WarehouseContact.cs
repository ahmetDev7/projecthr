using System.ComponentModel.DataAnnotations;

public class WarehouseContact : BaseModel
{
    public WarehouseContact() { }
    public WarehouseContact(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }

    [Required]
    public Guid? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}