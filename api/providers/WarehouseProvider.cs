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

        if (request == null) throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        if (request.ContactId != null)
        {
            if (_contactProvider.GetById(request.ContactId.Value) == null) throw new ApiFlowException("ContactID does not exist");
        }

        if (request.AddressId != null)
        {
           if(_addressProvider.GetById(request.AddressId.Value) == null) throw new ApiFlowException("AddressID does not exist");
        }

        if (request.ContactId == null && request.Contact == null)
            throw new Exception("Either contact_id or contact fields must be filled");

        if (request.AddressId == null && request.Address == null)
            throw new Exception("Either address_id or address fields must be filled");

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

        DBUtil.SaveChanges(_db, "Location not stored");

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
