public class LocationsProvider : ICRUD<Location>
{
    private readonly AppDbContext _db;
    public LocationsProvider(AppDbContext db){
        _db = db;
    }
    public Location? Create<IDTO>(IDTO dto)
    {
        LocationDTO? req = dto as LocationDTO;
        if (req == null) throw new ApiFlowException("Could not process create location request. Save new location failed.");
        
        Location newLocation = new()
        {
            Row = req.Row,
            Rack =req.Rack,
            Shelf =req.Shelf,
            WarehouseId = req.WarehouseId
        };
        
        _db.Locations.Add(newLocation);
        
        DBUtil.SaveChanges(_db, "Location not stored");
        
        return newLocation;      
    }

    public Location? Delete(Guid id)
    {
        Location? foundLocation = GetById(id);
        if(foundLocation == null) return null;

        _db.Locations.Remove(foundLocation);
        
        DBUtil.SaveChanges(_db, "Location not deleted");

        return foundLocation;
    }

    public List<Location>? GetAll() => _db.Locations.ToList();

    public Location? GetById(Guid id) => _db.Locations.FirstOrDefault(l => l.Id == id );

    public Location? Update(Guid id)
    {
        throw new NotImplementedException();
    }
}
