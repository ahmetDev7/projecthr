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
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransferItem> TransferItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentStatus).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.PaymentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.TransferMode).HasConversion<string>();
        modelBuilder.Entity<Transfer>().Property(t => t.TransferStatus).HasConversion<string>();


        modelBuilder.Entity<Location>()
            .HasMany(e => e.TransfersFrom)
            .WithOne(e => e.TransferFrom)
            .HasForeignKey(e => e.TransferFromId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<Location>()
            .HasMany(e => e.TransfersTo)
            .WithOne(e => e.TransferTo)
            .HasForeignKey(e => e.TransferToId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}