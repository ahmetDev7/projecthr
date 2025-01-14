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
            db.ItemLines.AddRange(GetItemLines());
            db.Locations.AddRange(GetLocations());
            db.Addresses.AddRange(GetAddresses());
            db.Warehouses.AddRange(GetWarehouses());
            db.Items.AddRange(GetItems());
            db.Contacts.AddRange(GetContacts());
            db.Suppliers.AddRange(GetSuppliers());
            db.Docks.AddRange(GetDocks());
            db.Clients.AddRange(GetClients());
            // db.Orders.AddRange(GetOrders());
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

        private static List<ItemLine> GetItemLines()
        {
            return new List<ItemLine>(){
                new ItemLine(newInstance: true) { Id = Guid.Parse("dac7430d-c2c9-48f3-ad74-f443649c0c43"), Name = "Item Line 1", Description = "Description Item Line 1"},
                new ItemLine(newInstance: true) { Id = Guid.Parse("5555260d-e59d-442d-8dfc-305d53a8e4f5"), Name = "Item Line 2", Description = "Description Item Line 2"},
                new ItemLine(newInstance: true) { Id = Guid.Parse("1a460afb-7922-4eec-a633-cebea9b9f3fb"), Name = "Item Line 3", Description = "Description Item Line 3"}
            };
        }
        private static List<Item> GetItems()
        {
            return new List<Item>()
            {
                new Item(newInstance:true)
                {
                    Id = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                    Code = "123",
                    Description = "123",
                    ShortDescription = "123",
                    UpcCode = "123",
                    ModelNumber = "2",
                    CommodityCode = "55",
                    UnitPurchaseQuantity = 20,
                    UnitOrderQuantity = 20,
                    PackOrderQuantity = 20,
                    SupplierReferenceCode = "AB20",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                    ItemTypeId = Guid.Parse("276b1f8f-f695-46f4-9db0-78ec3f358210"),
                    ItemLineId = Guid.Parse("dac7430d-c2c9-48f3-ad74-f443649c0c43")
                },
                new Item(newInstance:true)
                {
                    Id = Guid.Parse("ab868b64-2a27-451a-be78-105e824547be"),
                    Code = "321",
                    UpcCode = "321",
                    ModelNumber = "1",
                    UnitPurchaseQuantity = 2,
                    UnitOrderQuantity = 2,
                    PackOrderQuantity = 2,
                    SupplierReferenceCode = "02BA",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                    ItemTypeId = Guid.Parse("276b1f8f-f695-46f4-9db0-78ec3f358210"),
                    ItemLineId = Guid.Parse("dac7430d-c2c9-48f3-ad74-f443649c0c43")
                }
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
        private static List<Contact> GetContacts()
        {
            return new List<Contact>()
            {
                new Contact(newInstance:true)
                {
                    Id = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    Name = "Alperen",
                    Phone = "0644089743",
                    Email = "Dev@Gmail.com"
                }
            };
        }

        private static List<Warehouse> GetWarehouses()
        {
            return new List<Warehouse>()
            {
                new Warehouse(newInstance: true)
                {
                    Id = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                    Code = "123",
                    Name = "Amstelveen",
                    AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e")
                },
                new Warehouse(newInstance: true)
                {
                    Id = Guid.Parse("8798e409-e0b5-4575-a95d-2d8136d595ec"),
                    Code = "321",
                    Name = "Neevletsma",
                    AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e")
                }
            };


        }
        private static List<Supplier> GetSuppliers()
        {
            return new List<Supplier>()
            {
                new Supplier(newInstance:true)
                {
                    Id = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                    Code = "1234",
                    Name = "Rotto",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099")

                },
                new Supplier(newInstance:true)
                {
                    Id = Guid.Parse("1c989e40-9b2e-4cd7-bff2-abf42d977e27"),
                    Code = "4321",
                    Name = "Ottor",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099")

                }
            };
        }
        private static List<Dock> GetDocks()
        {
            return new List<Dock>()
            {
                new Dock(){Id = Guid.Parse("b1c4625d-a113-4d5a-bf19-34e6d695a67c"),WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49")},
                new Dock(){Id = Guid.Parse("33b9d661-89be-41e3-ace2-e9f9c505bde0"),WarehouseId = Guid.Parse("8798e409-e0b5-4575-a95d-2d8136d595ec")}
            };
        }
        private static List<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client(newInstance:true)
                {
                    Id = Guid.Parse("8f568644-4d30-4658-ab68-c80d0636ba8f"),
                    Name = "Jansen",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099"),
                },
                new Client(newInstance:true)
                {
                    Id = Guid.Parse("68b7ef68-b6a7-45de-a6f8-7656b7af44b7"),
                    Name = "Tim",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099"),
                }

            };
        }
        // private static List<Order> GetOrders()
        // {
        //     return new List<Order>()
        //     {
        //         new Order(newInstance:true)
        //         {
        //             Id = Guid.Parse("7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c"),
        //             OrderDate = DateTime.UtcNow,
        //             OrderStatus = OrderStatus.Pending,
        //             WarehouseId = Guid.Parse("8798e409-e0b5-4575-a95d-2d8136d595ec"),
        //             BillToClientId = Guid.Parse("68b7ef68-b6a7-45de-a6f8-7656b7af44b7"),
        //             OrderItems = new List<OrderItem>
        //             {
        //                 new OrderItem(newInstance: true)
        //                 {
        //                     Id = Guid.Parse("c92d1c2e-b81b-476d-8f11-76818140f7bc"),
        //                     ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
        //                     Amount = 2
        //                 },
        //                 new OrderItem(newInstance: true)
        //                 {
        //                     Id = Guid.Parse("f0b34d6a-8f11-48a5-b0ad-20b63b2cd19a"),
        //                     ItemId = Guid.Parse("ab868b64-2a27-451a-be78-105e824547be"),
        //                     Amount = 4
        //                 }
        //             }
        //         }
        //     };
        // }

    }
}