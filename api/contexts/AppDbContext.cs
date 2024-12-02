using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> db) : base(db) { }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Client> Clients { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentStatus).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.PaymentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.TransferMode).HasConversion<string>();
        // Relatie ShipToClient
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShipToClient)
            .WithMany(c => c.ShipToOrders)
            .HasForeignKey(o => o.ShipToClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relatie BillToClient
        modelBuilder.Entity<Order>()
            .HasOne(o => o.BillToClient)
            .WithMany(c => c.BillToOrders)
            .HasForeignKey(o => o.BillToClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}