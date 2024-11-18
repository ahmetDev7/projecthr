using Microsoft.EntityFrameworkCore;
using Model;
using Models.Location;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> db) : base(db) { }

    public DbSet<Warehouse> Warehouses {get; set;}
    public DbSet<Location> Locations {get; set;}
    public DbSet<Address> Addresses {get; set;}
    public DbSet<Contact> Contacts {get; set;}
    public DbSet<Item> Items {get; set;}
    public DbSet<Shipment> Shipments {get; set;}
    public DbSet<ItemGroup> ItemGroups {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Shipment-Item many-to-many relationship
        modelBuilder.Entity<ShipmentItem>()
            .HasKey(si => new { si.ShipmentId, si.ItemId });

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(si => si.Shipment)
            .WithMany(s => s.ShipmentItems)
            .HasForeignKey(si => si.ShipmentId);

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(si => si.Item)
            .WithMany(i => i.ShipmentItems)
            .HasForeignKey(si => si.ItemId);
    }
} 