using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using SQLitePCL;

namespace api.IntegrationTests
{
    public class Seeding
    {
        public static void IntializeTestDB(AppDbContext db)
        {
            db.ItemTypes.AddRange(GetItemTypes());
            db.ItemGroups.AddRange(GetItemGroups());
            db.ItemLines.AddRange(GetItemLines());
            db.Locations.AddRange(GetLocations());
            db.Addresses.AddRange(GetAddresses());
            db.Warehouses.AddRange(GetWarehouses());
            db.Items.AddRange(GetItems());
            db.Contacts.AddRange(GetContacts());
            db.Suppliers.AddRange(GetSuppliers());
            db.Docks.AddRange(GetDocks());
            db.Clients.AddRange(GetClients());
            db.Transfers.AddRange(GetTransfers());
            db.Inventories.AddRange(GetInventories());
            db.Shipments.AddRange(GetShipments());
            db.Orders.AddRange(GetOrders());
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

        private static List<ItemGroup> GetItemGroups()
        {
            return new List<ItemGroup>(){
                new ItemGroup(newInstance: true) { Id = Guid.Parse("4604084f-a55f-484f-8707-feae90c72fcd"), Name = "Item Group 1", Description = "Description Item Group 1" },
                new ItemGroup(newInstance: true) { Id = Guid.Parse("4428de87-dd7f-4879-823b-ec9f97e50add"), Name = "Item Group 2", Description = "Description Item Group 2" },
                new ItemGroup(newInstance: true) { Id = Guid.Parse("00654a9f-83c1-49db-acb0-6d908c0520fc"), Name = "Item Group 3", Description = "Description Item Group 3" }
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
                    ItemGroupId = Guid.Parse("4604084f-a55f-484f-8707-feae90c72fcd"),
                    ItemLineId = Guid.Parse("dac7430d-c2c9-48f3-ad74-f443649c0c43")
                },
                    new Item(newInstance:true)
                {
                    Id = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e651"),
                    Code = "222",
                    Description = "222",
                    ShortDescription = "222",
                    UpcCode = "222",
                    ModelNumber = "222",
                    CommodityCode = "222",
                    UnitPurchaseQuantity = 20,
                    UnitOrderQuantity = 20,
                    PackOrderQuantity = 20,
                    SupplierReferenceCode = "AB202424",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
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
                    ItemGroupId = Guid.Parse("4604084f-a55f-484f-8707-feae90c72fcd"),
                    ItemLineId = Guid.Parse("dac7430d-c2c9-48f3-ad74-f443649c0c43")
                }
                ,
                new Item(newInstance:true)
                {
                    Id = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc3d"),
                    Code = "3213244",
                    UpcCode = "3213244",
                    ModelNumber = "3213244",
                    UnitPurchaseQuantity = 2,
                    UnitOrderQuantity = 2,
                    PackOrderQuantity = 2,
                    SupplierReferenceCode = "02BA",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                },
                new Item(newInstance:true)
                {
                    Id = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc4d"), // for update inventory
                    Code = "323",
                    UpcCode = "323",
                    ModelNumber = "323",
                    UnitPurchaseQuantity = 323,
                    UnitOrderQuantity = 2323,
                    PackOrderQuantity = 323,
                    SupplierReferenceCode = "02BA",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                },
                new Item(newInstance:true)
                {
                    Id = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc5d"), // for delete inventory
                    Code = "89789",
                    UpcCode = "89789",
                    ModelNumber = "89789",
                    UnitPurchaseQuantity = 22,
                    UnitOrderQuantity = 82,
                    PackOrderQuantity = 29,
                    SupplierReferenceCode = "02342BA",
                    SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
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
                },
                new Location(newInstance: true)
                {
                    Id = Guid.Parse("68b1efa8-8cbe-4dae-867f-40384954c5cd"),
                    Row = "12",
                    Rack = "13",
                    Shelf = "25",
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
                },
                new Address(newInstance: true)
                {
                    Id = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61088"),
                    Street = "Street 3",
                    HouseNumber = "3",
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
                },
                new Contact(newInstance:true)
                {
                    Id = Guid.Parse("77366127-2bb6-4656-ac24-760a27623a08"),
                    Name = "Jan",
                    Phone = "06549898814",
                    Email = "jan@gmail.com"
                },
                new Contact(newInstance:true) // NOTE: To be deleted
                {
                    Id = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a10"),
                    Name = "Test",
                    Phone = "0644089743",
                    Email = "Test@Gmail.com"
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
                ,
                new Warehouse(newInstance: true) // to be deleted
                {
                    Id = Guid.Parse("832a3030-4297-4e97-ba5e-c563241ec982"),
                    Code = "82346",
                    Name = "Dutch Korps",
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

                },
                new Supplier(newInstance:true)
                {
                    Id = Guid.Parse("1c989e40-9b2e-4cd7-bff2-abf42d977e88"),
                    Code = "37462",
                    Name = "Rick Lowman",
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
                },
                new Client(newInstance:true)
                {
                    Id = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                    Name = "Client 2",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099"),
                },
                new Client(newInstance:true)
                {
                    Id = Guid.Parse("39fbea38-bb4b-49db-b9dc-fe1aec620441"),
                    Name = "Client 3",
                    ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                    AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099"),
                }

            };
        }
        private static List<Order> GetOrders()
        {
            return new List<Order>()
            {
                new Order(newInstance:true)
                {
                    Id = Guid.Parse("7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c"),
                    OrderDate = DateTime.UtcNow,
                    RequestDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Pending,
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                    BillToClientId = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem(newInstance: true)
                        {
                            Id = Guid.Parse("c92d1c2e-b81b-476d-8f11-76818140f7bc"),
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 2
                        }
                    }
                },
                new Order(newInstance:true)
                {
                    Id = Guid.Parse("9eb03425-c377-443b-bf5a-3010de9f4d7d"), // to update
                    OrderDate = DateTime.UtcNow,
                    RequestDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Being_Packed,
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                    BillToClientId = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem(newInstance: true)
                        {
                            Id = Guid.Parse("7b2064ae-9a05-4434-aaff-e941d08d4f9c"),
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 33
                        }
                    }
                },
                new Order(newInstance:true)
                {
                    Id = Guid.Parse("797146d4-d13c-4c00-b89d-fb04980f728d"), // to delete
                    OrderDate = DateTime.UtcNow,
                    RequestDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Being_Packed,
                    WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                    BillToClientId = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem(newInstance: true)
                        {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 17
                        }
                    }
                }
            };
        }
        private static List<Shipment> GetShipments()
        {
            return new List<Shipment>()
            {
                new Shipment(newInstance:true)
                {
                    Id = Guid.Parse("9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74"), // for linking with an order
                    ShipmentType = ShipmentType.O,
                    CarrierCode = "010101",
                    ServiceCode = "304402",
                    PaymentType = PaymentType.Manual,
                    TransferMode = TransferMode.Sea,
                    ShipmentStatus = ShipmentStatus.Plan,
                    OrderDate = new DateTime().ToUniversalTime(),
                    RequestDate = new DateTime().ToUniversalTime(),
                    ShipmentItems = new List<ShipmentItem>(){
                        new ShipmentItem() {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 21
                        }
                    }
                },
                new Shipment(newInstance:true)
                {
                    Id = Guid.Parse("67833cb8-9d20-4c13-922b-26c354a97a9f"), // to status deliverd
                    ShipmentType = ShipmentType.I,
                    CarrierCode = "894357",
                    ServiceCode = "8923478-DB",
                    PaymentType = PaymentType.Automatic,
                    TransferMode = TransferMode.Air,
                    ShipmentStatus = ShipmentStatus.Plan
                },
                new Shipment(newInstance:true) // to delete
                {
                    Id = Guid.Parse("4786f6b5-b2ed-4ed9-9cff-7d5333e4925e"),
                    ShipmentType = ShipmentType.I,
                    CarrierCode = "894357",
                    ServiceCode = "8923478-DB",
                    PaymentType = PaymentType.Automatic,
                    TransferMode = TransferMode.Ground,
                    ShipmentStatus = ShipmentStatus.Delivered
                }
            };
        }

        private static List<Transfer> GetTransfers()
        {
            return new List<Transfer>()
            {
                new Transfer(newInstance:true)
                {
                    Id = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f203e"),
                    TransferFromId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                    TransferToId = Guid.Parse("e6786fad-435b-460f-b6dd-11dd32b3b6a6"),
                    TransferStatus = TransferStatus.Pending,
                    Reference = "TST-0273",
                    TransferItems = new List<TransferItem>()
                    {
                        new TransferItem
                        {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 100
                        }
                    }
                },
                new Transfer(newInstance:true) // to be deleted
                {
                    Id = Guid.Parse("4e889392-bd86-4305-a31f-db5d8d0ff17a"),
                    TransferFromId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                    TransferToId = Guid.Parse("e6786fad-435b-460f-b6dd-11dd32b3b6a6"),
                    TransferStatus = TransferStatus.Pending,
                    Reference = "TST-024872",
                    TransferItems = new List<TransferItem>()
                    {
                        new TransferItem
                        {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 56
                        }
                    }
                }
            };
        }

        private static List<Inventory> GetInventories()
        {
            return new List<Inventory>()
            {
                new Inventory(newInstance:true)
                {
                    Id = Guid.Parse("722eec5c-9de0-4993-8aea-3b473ec30d22"),
                    Description = "INVENTORY ITEM INTEGRATION TEST DESCRIPTION",
                    ItemReference = "INVENTORY ITEM INTEGRATION TEST REFERENCE",
                    ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                    InventoryLocations = new List<InventoryLocation>()
                    {
                        new InventoryLocation
                        {
                            LocationId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                            OnHandAmount = 500
                        }
                    }
                },
                new Inventory(newInstance:true)
                {
                    Id = Guid.Parse("722eec5c-9de0-4993-8aea-3b473ec30d79"),
                    Description = "INVENTORY ITEM INTEGRATION TEST DESCRIPTION 002",
                    ItemReference = "INVENTORY ITEM INTEGRATION TEST REFERENCE 002",
                    ItemId = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc5d")
                }
            };
        }



    }
}