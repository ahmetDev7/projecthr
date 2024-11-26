using System.ComponentModel.DataAnnotations;

public class Supplier : BaseModel
{
    public Supplier() { }
    public Supplier(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Reference { get; set; }
    // foreing key
    [Required]
    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }
    [Required]
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
}