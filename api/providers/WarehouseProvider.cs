using CargoHub.DTOs;

public class WarehouseProvider : ICRUD<Warehouse>
{
    private readonly AppDbContext _db;
    private readonly AddressProvider _addressProvider;
    private readonly ContactProvider _contactProvider;

    public WarehouseProvider(AppDbContext db, AddressProvider addressProvider, ContactProvider contactProvider)
    {
        _db = db;
        _addressProvider = addressProvider;
        _contactProvider = contactProvider;
    }

    public Warehouse? Create<IDTO>(IDTO newElement)
    {
        WarehouseDTO? request = newElement as WarehouseDTO;
        if (request == null) throw new Exception("Request invalid");

        // Case 1: contact_id is ingevuld
        // if contact_id is ingevuld
        // check als contact_id bestaat zo niet return error

        // Case 2: als contact_id niet is ingevuld dan is contact field verplicht en moet daar naar gekeken worden om nieuwe contact te maken

        // Case 3: address_id is ingevuld
        // if address_id is ingevuld
        // check als address_id bestaat zo niet return error

        // Case 4: als address_id niet is ingevuld dan is contact field verplicht en moet daar naar gekeken worden om nieuwe contact te maken


        // TODO: contact maken in de database op basis van de request en dan de contact.id pakken nadat de contact is gemaakt in de database 
        // als deze add contact null is return new exception error
        // Create a new Warehouse entity from the WarehouseDTO

        if (request.Contact == null) throw new Exception("Address must be filled");
        Contact? newContact = _contactProvider.Create<ContactDTO>(request.Contact);
        if(newContact == null) throw new Exception("An error occurred while saving the warehouse contact");

        // Create Address based on request.Address
        if (request.Address == null) throw new Exception("Address must be filled");
        Address? newAddress = _addressProvider.Create<AddressDTO>(request.Address);

        if (newAddress == null) throw new Exception("An error occurred while saving the warehouse address");

        Warehouse newWarehouse = new()
        {
            Code = request.Code,
            Name = request.Name,
            ContactId = newContact.Id,
            AddressId = newAddress.Id,
        };

        _db.Warehouses.Add(newWarehouse);

        if (_db.SaveChanges() < 1)
        {
            throw new Exception("An error occurred while saving the warehouse");
        }

        return newWarehouse;
    }


    public Warehouse Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Warehouse> GetAll()
    {
        throw new NotImplementedException();
    }

    public Warehouse GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Warehouse Update(Guid id)
    {
        throw new NotImplementedException();
    }
}
