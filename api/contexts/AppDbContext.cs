using Microsoft.EntityFrameworkCore;
using Model;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> db) : base(db) { }

    public DbSet<Warehouse> Warehouses {get; set;}
    public DbSet<Location> Locations {get; set;}
    public DbSet<Address> Addresses {get; set;}
    public DbSet<Contact> Contacts {get; set;}
    public DbSet<Item> Items {get; set;}
    public DbSet<ItemGroup> ItemGroups {get; set;}
} 