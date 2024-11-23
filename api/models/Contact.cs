using System.ComponentModel.DataAnnotations;

public class Contact : BaseModel
{
    public Contact() { }
    public Contact(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Phone { get; set; }
    public string? Email { get; set; }

    // Navigation property for warehouses
    public ICollection<Warehouse>? Warehouses { get; set; }
}
