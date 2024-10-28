//Why ICollection? See MS docs: https://learn.microsoft.com/en-us/ef/core/modeling/relationships

public class Warehouse
{

    public Warehouse()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign Key Relationships
    public required Guid ContactId { get; set; }
    public Contact? Contact { get; set; }

    public required Guid AddressId { get; set; }
    public Address? Address { get; set; }

    // Navigation property for locations
    public ICollection<Location>? Locations { get; set; }
}