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
        var request = newElement as WarehouseDTO ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        if (request.ContactId == null && request.Contact == null)
            throw new ApiFlowException("Either contact_id or contact fields must be filled");

        if (request.AddressId == null && request.Address == null)
            throw new ApiFlowException("Either address_id or address fields must be filled");

        var relatedContact = GetOrCreateContact(request);
        var relatedAddress = GetOrCreateAddress(request);

        if (relatedContact == null || relatedAddress == null)
            throw new ApiFlowException("Failed to process address or contact");

        // Create the new Warehouse entry
        var newWarehouse = new Warehouse
        {
            Code = request.Code,
            Name = request.Name,
            ContactId = relatedContact.Id,
            AddressId = relatedAddress.Id
        };

        _db.Warehouses.Add(newWarehouse);
        DBUtil.SaveChanges(_db, "Location not stored");

        return newWarehouse;
    }

    private Contact? GetOrCreateContact(WarehouseDTO request)
    {
        if (request.ContactId != null)
            return _contactProvider.GetById(request.ContactId.Value)
                   ?? throw new ApiFlowException("contact_id does not exist");

        return request.Contact != null
               ? _contactProvider.Create<ContactDTO>(request.Contact)
               : throw new ApiFlowException("An error occurred while saving the warehouse contact");
    }

    private Address? GetOrCreateAddress(WarehouseDTO request)
    {
        if (request.AddressId != null)
            return _addressProvider.GetById(request.AddressId.Value)
                   ?? throw new ApiFlowException("address_id does not exist");

        return request.Address != null
               ? _addressProvider.Create<AddressDTO>(request.Address)
               : throw new ApiFlowException("An error occurred while saving the warehouse address");
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
