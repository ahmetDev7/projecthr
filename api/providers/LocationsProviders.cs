using FluentValidation;
using Models.Location;

public class LocationsProvider : ICRUD<Location>
{
    private readonly AppDbContext _db;
    private readonly IValidator<Location> _locationValidator;
    public LocationsProvider(AppDbContext db, IValidator<Location> locationValidator)
    {
        _db = db;
        _locationValidator = locationValidator;
    }
    public Location? Create<IDTO>(IDTO dto)
    {
        LocationDTO? req = dto as LocationDTO;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");

        // TODO: if warehouse does not exists return apiFlowException
        // req.WarehouseId

        Location newLocation = new Location
        {
            Row = req.Row,
            Rack = req.Rack,
            Shelf = req.Shelf,
            WarehouseId = req.WarehouseId.Value,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        validateLocation(newLocation);

        _db.Locations.Add(newLocation);

        DBUtil.SaveChanges(_db, "Location not stored");

        return newLocation;
    }

    public Location? Delete(Guid id)
    {
        Location? foundLocation = GetById(id);
        if (foundLocation == null) return null;

        _db.Locations.Remove(foundLocation);

        DBUtil.SaveChanges(_db, "Location not deleted");

        return foundLocation;
    }

    public List<Location>? GetAll() => _db.Locations.ToList();

    public Location? GetById(Guid id) => _db.Locations.FirstOrDefault(l => l.Id == id);

    public Location? Update<IDTO>(Guid id, IDTO dto)
    {
        LocationDTO? req = dto as LocationDTO;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");

        Location? foundLocation = GetById(id);
        if (foundLocation == null) return null;

        if (!string.IsNullOrEmpty(req.Row)) foundLocation.Row = req.Row;
        if (!string.IsNullOrEmpty(req.Rack)) foundLocation.Rack = req.Rack;
        if (!string.IsNullOrEmpty(req.Shelf)) foundLocation.Shelf = req.Shelf;
        if (req.WarehouseId.HasValue) foundLocation.WarehouseId = req.WarehouseId.Value;

        foundLocation.UpdatedAt = DateTime.UtcNow;

        // TODO: if warehouse does not exists return apiFlowException
        // req.WarehouseId

        validateLocation(foundLocation);

        DBUtil.SaveChanges(_db, "Location not updated");

        return foundLocation;
    }

    private void validateLocation(Location location) => _locationValidator.ValidateAndThrow(location);

}

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(location => location.Row)
            .NotNull().WithMessage("row is required.")
            .NotEmpty().WithMessage("row name cannot be empty.");

        RuleFor(location => location.Rack)
            .NotNull().WithMessage("rack is required.")
            .NotEmpty().WithMessage("rack name cannot be empty.");

        RuleFor(location => location.Shelf)
            .NotNull().WithMessage("shelf is required.")
            .NotEmpty().WithMessage("shelf name cannot be empty.");

        RuleFor(location => location.WarehouseId)
            .NotNull().WithMessage("warehouse_id is required.")
            .NotEmpty().WithMessage("warehouse_id name cannot be empty.");
    }
}
