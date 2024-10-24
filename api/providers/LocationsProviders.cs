using System.Text.Json;

public class LocationsProvider : ICRUD<Location>
{
    private readonly string _filePath;

    public LocationsProvider(string filePath)
    {
        _filePath = filePath;
    }

    public List<Location> GetAll()
    {
        var jsonString = File.ReadAllText(_filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            AllowTrailingCommas = true 
        };
        List<Location>? decodedLocations = JsonSerializer.Deserialize<List<Location>>(jsonString, options);
        if(decodedLocations != null){
            return decodedLocations;
        }
        throw new Exception($"Locaties niet gevonden.");
    }

    public Location GetById(int id)
    {
        List<Location> allLocation = GetAll();
        Location? location = allLocation.FirstOrDefault(loc =>loc.Id == id);
        if(location != null){
            return location;
        }
        throw new Exception($"Locatie met id {id} niet gevonden.");
    }

    public Location Create()
    {
        throw new NotImplementedException();
    }

    public Location Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Location Update(int id)
    {
        throw new NotImplementedException();
    }
}
