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
    public Contact? Contact { get; set; }
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    //Timestamps
    public override DateTime CreatedAt { get; set; }
    public override DateTime UpdatedAt { get; set; }

    public override void SetTimeStamps()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public override void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}