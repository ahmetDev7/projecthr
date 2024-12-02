using DTO;
using FluentValidation;
using DTO.Contact;

public class WarehouseProvider : BaseProvider<Warehouse>
{
    private readonly AppDbContext _db;
    private readonly AddressProvider _addressProvider;
    private readonly ContactProvider _contactProvider;
    private IValidator<Warehouse> _WarehouseValidator;

    public WarehouseProvider(AppDbContext db, AddressProvider addressProvider, ContactProvider contactProvider, IValidator<Warehouse> validator) : base(db)
    {
        _db = db;
        _addressProvider = addressProvider;
        _contactProvider = contactProvider;
        _WarehouseValidator = validator;
    }

    public Warehouse? Create<IDTO>(IDTO newElement)
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
        if(foundWarehouse == null) return null;

        _db.Warehouses.Remove(foundWarehouse);
        
        DBUtil.SaveChanges(_db, "Warehouse not deleted");

        return foundWarehouse;
    }

    public List<Warehouse> GetAll()
    {
        return _db.Warehouses.ToList();
    }

    public Warehouse? GetById(Guid id) => _db.Warehouses.FirstOrDefault(l => l.Id == id);

    public override Warehouse? Update(Guid id, BaseDTO updatedValues)
    {
        bool hasChanges = false;
        WarehouseRequest? req = updatedValues as WarehouseRequest;
        if (req == null) throw new ApiFlowException("Could not process update warehouse request. Update new warehouse failed.");

        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null) return null;

        var relatedAddress = GetOrCreateAddress(req);
        var relatedContact = GetOrCreateContact(req);

        if (!string.IsNullOrEmpty(req.Code) && req.Code != foundWarehouse.Code)
        {
            foundWarehouse.Code = req.Code;
            hasChanges = true;
        }
        if(req.Name != foundWarehouse.Name)
        {
            foundWarehouse.Name = req.Name;
            hasChanges = true;
        }
        if(req.Contact != null || req.ContactId != null)
        {
            foundWarehouse.ContactId = relatedContact.Id;
            hasChanges = true;
        }
        if(req.Address != null || req.AddressId != null)
        {
            foundWarehouse.AddressId = relatedAddress.Id;
            hasChanges = true;
        }

        if (hasChanges) foundWarehouse.SetUpdatedAt();

        SaveToDBOrFail();

        return foundWarehouse;
    }

    public List<Location> GetLocationsByWarehouseId(Guid warehouseId)
    {
        var locationsOfspecificWarehouse = _db.Locations.Where(l => l.WarehouseId == warehouseId).ToList();
        if (locationsOfspecificWarehouse.Count == 0)
            throw new ApiFlowException("No locations found for this warehouse");

        return locationsOfspecificWarehouse;
    }

    protected override void ValidateModel(Warehouse model) => _WarehouseValidator.ValidateAndThrow(model);
}
