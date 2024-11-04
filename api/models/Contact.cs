public class Contact
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }

    // Navigation property for warehouses
    public ICollection<Warehouse>? Warehouses { get; set; }
}
