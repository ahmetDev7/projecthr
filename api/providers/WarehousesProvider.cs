using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class WarehousesProvider : BaseProvider<Warehouse>
{
    private IValidator<Warehouse> _WarehouseValidator;

    public WarehousesProvider(AppDbContext db, AddressProvider addressProvider, ContactProvider contactProvider, IValidator<Warehouse> validator) : base(db)
    {
        _WarehouseValidator = validator;
    }
    public override Warehouse? GetById(Guid id) => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).FirstOrDefault(l => l.Id == id);

    public override List<Warehouse> GetAll() => _db.Warehouses.Include(w => w.Address).Include(w => w.Contact).ToList();

    public override Warehouse? Create(BaseDTO newElement)
    {
        WarehouseRequest? req = newElement as WarehouseRequest ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Warehouse? newWarehouse = new Warehouse(newInstance: true)
            {
                Code = req.Code,
                Name = req.Name,
                AddressId = req.AddressId,
                ContactId = req.ContactId,
            };
            ValidateModel(newWarehouse);
            _db.Warehouses.Add(newWarehouse);

            SaveToDBOrFail();
            transaction.Commit();


            return GetById(newWarehouse.Id);
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
        foundWarehouse.AddressId = req.AddressId;
        foundWarehouse.ContactId = req.ContactId;

        ValidateModel(foundWarehouse);
        foundWarehouse.SetUpdatedAt();

        SaveToDBOrFail();

        return GetById(foundWarehouse.Id);
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