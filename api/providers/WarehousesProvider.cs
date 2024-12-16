using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class WarehousesProvider : BaseProvider<Warehouse>
{
    private readonly AddressProvider _addressProvider;
    private readonly ContactProvider _contactProvider;
    private readonly DocksProvider _docksProvider;
    private IValidator<Warehouse> _WarehouseValidator;

    public WarehousesProvider(AppDbContext db, AddressProvider addressProvider, ContactProvider contactProvider, DocksProvider docksProvider, IValidator<Warehouse> validator) : base(db)
    {
        _addressProvider = addressProvider;
        _contactProvider = contactProvider;
        _docksProvider = docksProvider;
        _WarehouseValidator = validator;
    }
    public override Warehouse? GetById(Guid id) => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).FirstOrDefault(l => l.Id == id);

    public override List<Warehouse> GetAll() => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).ToList();

    public Warehouse? Create<BaseDTO>(BaseDTO newElement)
    {
        WarehouseRequest? req = newElement as WarehouseRequest ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Address? relatedAddress = _addressProvider.GetOrCreateAddress(address: req.Address, addressId: req.AddressId);
            Contact? relatedContact = _contactProvider.GetOrCreateContact(contact: req.Contact, contactId: req.ContactId);

            Warehouse? newWarehouse = new Warehouse(newInstance: true)
            {
                Code = req.Code,
                Name = req.Name,
                AddressId = relatedAddress != null ? relatedAddress.Id : null,
                ContactId = relatedContact != null ? relatedContact.Id : null,
            };

            _db.Warehouses.Add(newWarehouse);
            SaveToDBOrFail();
            _docksProvider.InternalCreate(newWarehouse.Id); // Creates an specific dock inside warehouse (only for create)
            transaction.Commit();

            newWarehouse.Contact = relatedContact;
            newWarehouse.Address = relatedAddress;

            return newWarehouse;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public override Warehouse? Update(Guid id, BaseDTO updatedValues)
    {
        WarehouseRequest? req = updatedValues as WarehouseRequest ?? throw new ApiFlowException("Could not process update warehouse request. Update failed.");

        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null)
            throw new ApiFlowException($"Warehouse with ID {id} does not exist.", StatusCodes.Status404NotFound);

        foundWarehouse.Name = req.Name;
        foundWarehouse.Code = req.Code;

        Guid? oldAddressId = foundWarehouse.AddressId;
        Guid? oldContactId = foundWarehouse.AddressId;

        // TODO: UPDATE CONTACT AND ADDRESS (coming soon with address and contact rework)       

        foundWarehouse.SetUpdatedAt();

        ValidateModel(foundWarehouse);
        SaveToDBOrFail();

        return foundWarehouse;
    }

    public override Warehouse? Delete(Guid id)
    {
        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null) throw new ApiFlowException($"Warehouse with ID {id} does not exist.", StatusCodes.Status404NotFound);

        _db.Warehouses.Remove(foundWarehouse);
        SaveToDBOrFail();
        return foundWarehouse;
    }

    public List<Location> GetLocationsByWarehouseId(Guid warehouseId) => _db.Locations.Where(l => l.WarehouseId == warehouseId).ToList();

    protected override void ValidateModel(Warehouse model) => _WarehouseValidator.ValidateAndThrow(model);
}