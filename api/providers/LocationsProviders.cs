using System.Collections.Generic;
using System.IO;
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

        if (decodedLocations != null)
        {
            Console.WriteLine(decodedLocations);
            foreach (var l in decodedLocations)
            {
                Console.WriteLine(l.Name);
            }
        }

        return decodedLocations;
    }

    public Location GetById()
    {
        throw new NotImplementedException();
    }

    public Location Create()
    {
        throw new NotImplementedException();
    }

    public Location Delete()
    {
        throw new NotImplementedException();
    }

    public Location Update()
    {
        throw new NotImplementedException();
    }
}
