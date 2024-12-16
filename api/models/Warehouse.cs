//Why ICollection? See MS docs: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
using System.ComponentModel.DataAnnotations;

public class Warehouse : BaseModel
{
    public Warehouse() { }
    public Warehouse(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public Guid ContactId { get; set; } // Foreign Key Relationships
    public Contact? Contact { get; set; }
    [Required]
    public Guid AddressId { get; set; } // Foreign Key Relationships
    public Address? Address { get; set; }

    // Navigation property for locations
    public ICollection<Location>? Locations { get; set; }
}