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
        if (newElement is not WarehouseDTO request) throw new Exception("Request invalid");

        // Check if warehouse already exists
        if (_db.Warehouses.Any(w => w.Name == request.Name))
            throw new Exception("Warehouse already exists");

        // Case 1 and 2 and 5: Validate ContactId and AddressId if provided
        if (request.ContactId != null)
        {
            var contact = _db.Contacts.Find(request.ContactId);
            if (contact == null) throw new Exception("ContactID does not exist");
        }

        if (request.AddressId != null)
        {
            var address = _db.Addresses.Find(request.AddressId);
            if (address == null) throw new Exception("AddressID does not exist");
        }

        // Case 3: If ContactId is not provided and contact fields are empty
        if (request.ContactId == null && request.Contact == null)
            throw new Exception("Either contactId or contact fields must be filled");

        // Case 4: If AddressId is not provided and address fields are empty
        if (request.AddressId == null && request.Address == null)
            throw new Exception("Either addressId or address fields must be filled");

       
        // Create new Contact if provided
        Contact? newContact;
        if (request.Contact != null)
        {
            newContact = _contactProvider.Create<ContactDTO>(request.Contact);
            if (newContact == null) throw new Exception("An error occurred while saving the warehouse contact");
        }
        else
        {
            newContact = _db.Contacts.Find(request.ContactId);
        }

        // Create new Address if provided
        Address? newAddress;
        if (request.Address != null)
        {
            newAddress = _addressProvider.Create<AddressDTO>(request.Address);
            if (newAddress == null) throw new Exception("An error occurred while saving the warehouse address");
        }
        else
        {
            newAddress = _db.Addresses.Find(request.AddressId);
        }

        // Create the new Warehouse entry
        Warehouse newWarehouse = new()
        {
            Code = request.Code,
            Name = request.Name,
            ContactId = newContact.Id, 
            AddressId = newAddress.Id
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
