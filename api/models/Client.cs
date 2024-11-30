using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Client : BaseModel
{
    public Client() { }
    public Client(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Name { get; set; }
    [Required]
    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }
    [Required]
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
}
