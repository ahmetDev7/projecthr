using Model;
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
    public Guid ContactId { get; set; }
    public Contact? Contact { get; set; } // TODO: is required
    public Guid AddressId { get; set; } // TODO: is required
    public Address? Address { get; set; }


}