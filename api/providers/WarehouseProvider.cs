using Microsoft.EntityFrameworkCore;

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

    public Warehouse? Create<BaseDTO>(BaseDTO newElement)
    {
        var request = newElement as WarehouseRequest ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        if (request.ContactId == null && request.Contact == null)
            throw new ApiFlowException("Either contact_id or contact fields must be filled");

        if (request.AddressId == null && request.Address == null)
            throw new ApiFlowException("Either address_id or address fields must be filled");

        var relatedContact = GetOrCreateContact(request);
        var relatedAddress = GetOrCreateAddress(request);

        if (relatedContact == null || relatedAddress == null)
            throw new ApiFlowException("Failed to process address or contact");

        // Create the new Warehouse entry
        var newWarehouse = new Warehouse(newInstance: true)
        {
            Code = request.Code,
            Name = request.Name,
            ContactId = relatedContact.Id,
            AddressId = relatedAddress.Id
        };

        _db.Warehouses.Add(newWarehouse);
        DBUtil.SaveChanges(_db, "Location not stored");

        newWarehouse.Contact = relatedContact;
        newWarehouse.Address = relatedAddress;

        return newWarehouse;
    }

    private Contact? GetOrCreateContact(WarehouseRequest request)
    {
        if (request.ContactId != null)
            return _contactProvider.GetById(request.ContactId.Value)
                   ?? throw new ApiFlowException("contact_id does not exist");

        return request.Contact != null
               ? _contactProvider.Create(request.Contact)
               : throw new ApiFlowException("An error occurred while saving the warehouse contact");
    }

    private Address? GetOrCreateAddress(WarehouseRequest request)
    {
        if (request.AddressId != null)
            return _addressProvider.GetById(request.AddressId.Value)
                   ?? throw new ApiFlowException("address_id does not exist");

        return request.Address != null
               ? _addressProvider.Create(request.Address)
               : throw new ApiFlowException("An error occurred while saving the warehouse address");
    }

    public Warehouse? Delete(Guid id)
    {
        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null) return null;

        _db.Warehouses.Remove(foundWarehouse);
        DBUtil.SaveChanges(_db, "Warehouse not deleted");

        return foundWarehouse;
    }

    public List<Warehouse> GetAll() => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).ToList();

    public Warehouse? GetById(Guid id) => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).FirstOrDefault(l => l.Id == id);

    public Warehouse? Update<IDTO>(Guid id, IDTO dto)
    {
        throw new NotImplementedException();
    }

    public List<Location> GetLocationsByWarehouseId(Guid warehouseId) => _db.Locations.Where(l => l.WarehouseId == warehouseId).ToList();

}
