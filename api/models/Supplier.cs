using Model;
// Table suppliers {
//   id uuid [pk]
//   code varchar
//   name varchar
//   reference varchar
//   contact_id uuid [ref: > contacts.id]
//   address_id uuid [ref: > addresses.id]
//   created_at datetime
//   updated_at datetime
// }
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class Supplier : BaseModel
{
    public Supplier(){}
    public Supplier(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate){}
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Reference { get; set; }
    public Contact? Contact { get; set; }
    public Guid ContactId { get; set; }
    public Address? Address { get; set; }
    public Guid AddressId { get; set; }
}