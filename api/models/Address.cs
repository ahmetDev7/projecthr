using System.Text.Json.Serialization;

public class Address
{

    public Address()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
    public string? HouseNumberExtension { get; set; }
    public string? HouseNumberExtensionExtra { get; set; }
    public required string ZipCode { get; set; }
    public required string City { get; set; }
    public string? Province { get; set; }
    public required string CountryCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation property for warehouses
    [JsonIgnore]
    public ICollection<Warehouse>? Warehouses { get; set; }
}
