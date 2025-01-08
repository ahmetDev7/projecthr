using System.ComponentModel.DataAnnotations;

public class Contact : BaseModel
{
    public Contact() { }
    public Contact(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }

    public string? Function { get; set; }

    [Required]
    public string? Phone { get; set; }

    [Required]
    public string? Email { get; set; }

    public ICollection<WarehouseContact> WarehouseContacts { get; set; }
}
