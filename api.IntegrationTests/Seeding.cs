using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.IntegrationTests
{
    public class Seeding
    {
        public static void IntializeTestDB(AppDbContext db)
        {
            db.ItemTypes.AddRange(GetItemTypes());
            db.Locations.AddRange(GetLocations());
            db.Addresses.AddRange(GetAddresses());
            db.Warehouses.AddRange(GetWarehouses());
            db.SaveChanges();
        }

        private static List<ItemType> GetItemTypes()
        {
            return new List<ItemType>(){
                new ItemType(newInstance: true) { Id = Guid.Parse("276b1f8f-f695-46f4-9db0-78ec3f358210"), Name = "Item Type 1", Description = "Description Item Type 1" },
                new ItemType(newInstance: true) { Id = Guid.Parse("37f7653c-080b-4b1d-b1db-5b467fe29762"), Name = "Item Type 2", Description = "Description Item Type 2" },
                new ItemType(newInstance: true) { Id = Guid.Parse("23a60ea1-4471-4f1f-b0f5-a25527121647"), Name = "Item Type 3", Description = "Description Item Type 3" }
            };
        }
        private static List<Location> GetLocations()
        {   
            return new List<Location>(){
                new Location(newInstance: true)
                {
                    Id = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                    Row = "2",
                    Rack = "3",
                    Shelf = "5",
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49")
                },
                new Location(newInstance: true)
                { 
                    Id = Guid.Parse("e6786fad-435b-460f-b6dd-11dd32b3b6a6"),
                    Row = "2",
                    Rack = "3",
                    Shelf = "5",
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49")
                },
                new Location(newInstance: true)
                {
                    Id = Guid.Parse("68b1efa8-8cbe-4dae-867f-40384954c5cc"),
                    Row = "2",
                    Rack = "3",
                    Shelf = "5",
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49")
                }
            };
        }
        private static List<Address> GetAddresses()
        {
            return new List<Address>()
            {
                new Address(newInstance: true)
                {
                    Id = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e"),
                    Street = "Street 1",
                    HouseNumber = "1",
                    HouseNumberExtension = "A",
                    HouseNumberExtensionExtra = "B",
                    ZipCode = "1234AB",
                    City = "Amsterdam",
                    Province = "Noord-Holland",
                    CountryCode = "NL"
                },
                new Address(newInstance: true)
                {
                    Id = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099"),
                    Street = "Street 2",
                    HouseNumber = "2",
                    HouseNumberExtension = "B",
                    HouseNumberExtensionExtra = "C",
                    ZipCode = "4567AB",
                    City = "Amsterdam",
                    Province = "Noord-Holland",
                    CountryCode = "NL"
                }
            };
        }

        private static List<Warehouse>GetWarehouses()
        {
            return new List<Warehouse>()
            {
                new Warehouse(newInstance: true)
                {
                    Id = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                    Code = "123",
                    Name = "Amstelveen",
                    AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e"),
                }
            };

            
        }
        
    }
}