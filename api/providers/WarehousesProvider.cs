using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class WarehousesProvider : BaseProvider<Warehouse>
{
    public readonly DocksProvider _docksProvider;
    private IValidator<Warehouse> _WarehouseValidator;
    private IValidator<WarehouseRequest> _warehouseRequestValidator;


    public WarehousesProvider(AppDbContext db, DocksProvider docksProvider, IValidator<Warehouse> validator, IValidator<WarehouseRequest> warehouseRequestValidator) : base(db)
    {
        _warehouseRequestValidator = warehouseRequestValidator;
        _WarehouseValidator = validator;
        _docksProvider = docksProvider;
    }

    public override Warehouse? GetById(Guid id) => _db.Warehouses.Include(w => w.Address).Include(w => w.WarehouseContacts).ThenInclude(wc => wc.Contact).FirstOrDefault(l => l.Id == id);

    public override List<Warehouse> GetAll() => _db.Warehouses.Include(w => w.Address).Include(w => w.WarehouseContacts).ThenInclude(wc => wc.Contact).ToList();

    public override Warehouse? Create(BaseDTO newElement)
    {
        WarehouseRequest? req = newElement as WarehouseRequest ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");
        _warehouseRequestValidator.ValidateAndThrow(req);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Warehouse? newWarehouse = new Warehouse(newInstance: true)
            {
                Id = Guid.NewGuid(),
                Code = req.Code,
                Name = req.Name,
                AddressId = req.AddressId,
            };

            ValidateModel(newWarehouse);
            _db.Warehouses.Add(newWarehouse);
            SaveToDBOrFail();

            _docksProvider.InternalCreate(newWarehouse.Id); // Creates an specific dock inside warehouse (only for create)
            _db.WarehouseContacts.AddRange(
                req.ContactIds.Select(c => new WarehouseContact(newInstance: true)
                {
                    ContactId = c.Value,
                    WarehouseId = newWarehouse.Id,
                }).ToList()
            );
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

        _warehouseRequestValidator.ValidateAndThrow(req);

        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null)
            throw new ApiFlowException($"Warehouse with ID {id} does not exist.", StatusCodes.Status404NotFound);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            foundWarehouse.Name = req.Name;
            foundWarehouse.Code = req.Code;
            foundWarehouse.AddressId = req.AddressId;
            ValidateModel(foundWarehouse);
            _db.WarehouseContacts.RemoveRange(foundWarehouse.WarehouseContacts);
            foundWarehouse.SetUpdatedAt();
            SaveToDBOrFail();

            _db.WarehouseContacts.AddRange(
                req.ContactIds.Select(c => new WarehouseContact(newInstance: true)
                {
                    ContactId = c.Value,
                    WarehouseId = foundWarehouse.Id,
                }).ToList()
            );
            SaveToDBOrFail();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

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

    public List<DockItem> GetDockItemsByDockId(Guid dockId) => _db.DockItems.Include(di => di.Item).Where(di => di.DockId == dockId).ToList();

    protected override void ValidateModel(Warehouse model) => _WarehouseValidator.ValidateAndThrow(model);
}