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

    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransferItem> TransferItems { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Dock> Docks { get; set; }
    public DbSet<DockItem> DockItems { get; set; }
    public DbSet<InventoryLocation> InventoryLocations { get; set; }
    public DbSet<WarehouseContact> WarehouseContacts { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.ShipmentStatus).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.PaymentType).HasConversion<string>();
        modelBuilder.Entity<Shipment>().Property(s => s.TransferMode).HasConversion<string>();

        modelBuilder.Entity<Transfer>().Property(t => t.TransferStatus).HasConversion<string>();

        modelBuilder.Entity<TransferItem>()
            .HasKey(ti => new { ti.ItemId, ti.TransferId }); // Ensures that the same ItemId cannot be associated with the same TransferId more than once (prevents duplicate items within a single transfer) see: https://stackoverflow.com/questions/43070355/many-to-many-relation-and-unique-constraint-how-do-i-do-that-in-ef-core

        modelBuilder.Entity<Transfer>()
            .HasMany(e => e.TransferItems)  // Ensure TransferItems are linked to Transfer
            .WithOne(e => e.Transfer)
            .HasForeignKey(e => e.TransferId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete TransferItems when Transfer is deleted

        modelBuilder.Entity<Item>()
            .HasMany(e => e.TransferItems)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of Item if there are related Transfers

        modelBuilder.Entity<Location>()
            .HasMany(e => e.TransfersFrom)
            .WithOne(e => e.TransferFrom)
            .HasForeignKey(e => e.TransferFromId)
            .OnDelete(DeleteBehavior.Restrict);  // Restrict deletion of Location if there are related Transfers

        modelBuilder.Entity<Location>()
            .HasMany(e => e.TransfersTo)
            .WithOne(e => e.TransferTo)
            .HasForeignKey(e => e.TransferToId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of Location if there are related Transfers


        // Relation to ShipToClient
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShipToClient)
            .WithMany(c => c.ShipToOrders)
            .HasForeignKey(o => o.ShipToClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relation to BillToClient
        modelBuilder.Entity<Order>()
            .HasOne(o => o.BillToClient)
            .WithMany(c => c.BillToOrders)
            .HasForeignKey(o => o.BillToClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relation one to one with Dock | On delete Warehouse delete Dock
        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.Dock)
            .WithOne(d => d.Warehouse)
            .HasForeignKey<Dock>(d => d.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Warehouse>()
            .Navigation(w => w.Dock)
            .AutoInclude();
    }

}