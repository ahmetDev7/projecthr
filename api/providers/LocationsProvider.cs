using FluentValidation;

public class LocationsProvider : BaseProvider<Location>
{
    private readonly IValidator<Location> _locationValidator;
    public LocationsProvider(AppDbContext db, IValidator<Location> locationValidator) : base(db)
    {
        _locationValidator = locationValidator;
    }

    public override Location? GetById(Guid id) => _db.Locations.FirstOrDefault(l => l.Id == id);
    public override List<Location>? GetAll() => _db.Locations.ToList();

    public override Location? Create(BaseDTO createValues)
    {
        LocationRequest? req = createValues as LocationRequest;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");
        
        Location newLocation = new Location(newInstance: true)
        {
            Row = req.Row,
            Rack = req.Rack,
            Shelf = req.Shelf,
            WarehouseId = req.WarehouseId,
        };

        ValidateModel(newLocation);
        _db.Locations.Add(newLocation);
        SaveToDBOrFail("Location not stored");
        return newLocation;
    }

    public override Location? Update(Guid id, BaseDTO updatedValues)
    {
        LocationRequest? req = updatedValues as LocationRequest;
        if (req == null) throw new ApiFlowException("Could not process update location request. Save location failed.");

        Location? foundLocation = GetById(id);
        if (foundLocation == null) return null;

        foundLocation.Row = req.Row;
        foundLocation.Rack = req.Rack;
        foundLocation.Shelf = req.Shelf;
        foundLocation.WarehouseId = req.WarehouseId;


        ValidateModel(foundLocation);

        _db.Locations.Update(foundLocation);
        SaveToDBOrFail();
        return foundLocation;
    }

    public override Location? Delete(Guid id)
    {
        Location? foundLocation = GetById(id);
        if (foundLocation == null) return null;

        _db.Locations.Remove(foundLocation);
        SaveToDBOrFail("Location not deleted");

        return foundLocation;
    }

    protected override void ValidateModel(Location model) => _locationValidator.ValidateAndThrow(model);
}
