public class Location
{

    public Location()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public required string Row { get; set; }
    public required string Rack { get; set; }
    public required string Shelf { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign Key Relationship
    public required Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}
