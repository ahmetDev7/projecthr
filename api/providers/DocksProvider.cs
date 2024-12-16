using FluentValidation;

public class DocksProvider : BaseProvider<Dock>
{
    private IValidator<Dock> _dockValidator;

    public DocksProvider(AppDbContext db, IValidator<Dock> dockValidator) : base(db)
    {
        _dockValidator = dockValidator;
    }

    public void InternalCreate(Guid warehouseId)
    {
        Dock newDock = new Dock(newInstance: true) { WarehouseId = warehouseId };
        ValidateModel(newDock);
        _db.Docks.Add(newDock);
        SaveToDBOrFail();
    }

    protected override void ValidateModel(Dock model) => _dockValidator.ValidateAndThrow(model);
}