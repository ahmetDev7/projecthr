using System.ComponentModel.DataAnnotations;

public class Address : BaseModel
{
    public Address() { }
    public Address(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Street { get; set; }
    [Required]
    public string? HouseNumber { get; set; }
    public string? HouseNumberExtension { get; set; }
    public string? HouseNumberExtensionExtra { get; set; }
    [Required]
    public string? ZipCode { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? Province { get; set; }
    [Required]
    public string? CountryCode { get; set; }

    // Navigation property for warehouses
    public ICollection<Warehouse>? Warehouses { get; set; }
}
