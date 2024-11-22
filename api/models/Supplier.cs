using System.ComponentModel.DataAnnotations;


public class Supplier : BaseModel
{
    public Supplier() { }
    public Supplier(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    // foreing key
    public Guid ContactId { get; set; }
    public Contact? Contact { get; set; }
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
}