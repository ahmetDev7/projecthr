using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class LocationsProvider : BaseProvider<Location>
{
    private readonly IValidator<Location> _locationValidator;
    public LocationsProvider(AppDbContext db, IValidator<Location> locationValidator) : base(db)
    {
        _locationValidator = locationValidator;
    }

    private IQueryable<Location> GetLocationByIdQuery(bool includeWarehouse = false, bool includeInventory = false)
    {
        IQueryable<Location> query = _db.Locations.AsQueryable();
        if (includeWarehouse) query = query.Include(l => l.Warehouse);
        if (includeInventory) query = query.Include(l => l.Inventory);
        return query;
    }

    public override Location? GetById(Guid id) => GetLocationByIdQuery().FirstOrDefault(l => l.Id == id);

    public Location? GetById(Guid id, bool includeWarehouse = false, bool includeInventory = false) => GetLocationByIdQuery(includeWarehouse, includeInventory).FirstOrDefault(l => l.Id == id);

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

    public void FillOnHandAmount(Guid InventoryId, List<InventoryLocation>? inventoryLocations, bool saveToDb = false)
    {
        if (inventoryLocations == null || inventoryLocations.Count == 0) return;

        foreach (InventoryLocation inventoryLocation in inventoryLocations)
        {
            if (!inventoryLocation.LocationId.HasValue || !inventoryLocation.OnHand.HasValue) continue;

            Location? foundLocation = GetById(inventoryLocation.LocationId.Value);
            if (foundLocation == null) throw new ApiFlowException($"Location not found for id '{inventoryLocation.LocationId}'");

            if (foundLocation.InventoryId.HasValue)
            {
                throw new ApiFlowException($"The location '{inventoryLocation.LocationId}' already contains a stored item. Please select a different location.");
            }

            foundLocation.OnHand = inventoryLocation.OnHand.Value;
            foundLocation.InventoryId = InventoryId;
            foundLocation.SetUpdatedAt();
            ValidateModel(foundLocation);
            _db.Locations.Update(foundLocation);
        }

        if (saveToDb) SaveToDBOrFail("Failed to save inventory to the location with the specified on-hand amount. The inventory record was still saved successfully.");
    }

    public List<Location> GetLocationsByInventoryId(Guid inventoryId) => _db.Locations.Where(l => l.InventoryId == inventoryId).ToList();

    protected override void ValidateModel(Location model) => _locationValidator.ValidateAndThrow(model);
}
