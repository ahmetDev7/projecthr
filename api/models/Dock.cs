using System.ComponentModel.DataAnnotations;

public class Dock : BaseModel
{
    public const int CAPCITY = 50;

    public Dock() { }

    public Dock(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? WarehouseId {get; set;}

    public Warehouse? Warehouse {get; set;}    
}
