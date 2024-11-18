using Model;
using System.ComponentModel.DataAnnotations;


public class  Supplier : BaseModel
{
    public Supplier(){}
    public Supplier(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate){}
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Reference { get; set; }
    public ContactDTO? Contact { get; set; }
    public Guid ContactId { get; set; }
    public AddressDTO? Address { get; set; }
    public Guid AddressId { get; set; }
}