using DTO.Location;
using FluentValidation;
using Model;

public class LocationProvider : BaseProvider<Location>
{
    private  IValidator<Location> _locationValidator;
    public LocationProvider(AppDbContext db, IValidator<Location> validator): base(db)
    {
        _locationValidator = validator;
    }

        public Location? GetById(Guid id) => _db.Locations.FirstOrDefault(l => l.Id == id);

    public List<Location>? GetAll() => _db.Locations.ToList();

    public Location? Create(LocationReqest dto)
    {
        LocationReqest? req = dto as LocationReqest;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");

        Location newLocation = new Location
        {
            Row = req.Row,
            Rack = req.Rack,
            Shelf = req.Shelf,
            WarehouseId = req.WarehouseId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        ValidateModel(newLocation);

        _db.Locations.Add(newLocation);

        DBUtil.SaveChanges(_db, "Location not stored");

        return newLocation;
    }


    public Location? Update<IDTO>(Guid id, IDTO dto)
    {
        LocationReqest? req = dto as LocationReqest;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");

        Location? foundLocation = GetById(id);
        if (foundLocation == null) return null;

        if (!string.IsNullOrEmpty(req.Row)) foundLocation.Row = req.Row;
        if (!string.IsNullOrEmpty(req.Rack)) foundLocation.Rack = req.Rack;
        if (!string.IsNullOrEmpty(req.Shelf)) foundLocation.Shelf = req.Shelf;
        foundLocation.WarehouseId = req.WarehouseId;

        foundLocation.UpdatedAt = DateTime.UtcNow;

        ValidateModel(foundLocation);

        DBUtil.SaveChanges(_db, "Location not updated");

        return foundLocation;
    }
    public Location? Delete(Guid id)
    {
        Location? foundLocation = _db.Locations.FirstOrDefault(i => i.Id == id);
        if (foundLocation == null) return null;

        _db.Locations.Remove(foundLocation);

        DBUtil.SaveChanges(_db, "Location not deleted");

        return foundLocation;
    }

    protected override void ValidateModel(Location model) => _locationValidator.ValidateAndThrow(model);
}

