
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> db) : base(db) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore AddressDTO to prevent EF from treating it as an entity
        modelBuilder.Ignore<AddressDTO>();
        modelBuilder.Ignore<ContactDTO>();
    }

    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

}
