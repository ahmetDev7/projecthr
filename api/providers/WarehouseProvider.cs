using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

public class WarehouseProvider 
{
    private readonly string _filepath;
    public WarehouseProvider(string filepath)
    {
        _filepath = filepath;
    }
    
    public List<Warehouse> Getall()
    {
        var jsonString = File.ReadAllText(_filepath);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            AllowTrailingCommas = true 
        };
        List<Warehouse> ?decodedLocations = JsonSerializer.Deserialize<List<Warehouse>>(jsonString, options);

        return decodedLocations;
    }

    public void CreateWarehouse(Warehouse newWarehouse)
    {
        // 1. Haal de bestaande lijst van warehouses op
        List<Warehouse> warehouses = Getall();

        // 2. Controleer of er al een warehouse bestaat met hetzelfde Id of Name
        bool warehouseExists = warehouses.Any(w => w.id == newWarehouse.id || w.Name.Equals(newWarehouse.Name, StringComparison.OrdinalIgnoreCase));

        if (warehouseExists)
        {
            throw new InvalidOperationException($"A warehouse with Id {newWarehouse.id} or Name '{newWarehouse.Name}' already exists.");
        }

        // 3. Voeg het nieuwe warehouse toe aan de lijst
        warehouses.Add(newWarehouse);

        // 4. Seraliseer de bijgewerkte lijst terug naar JSON
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string updatedJson = JsonSerializer.Serialize(warehouses, options);

        // 5. Schrijf de bijgewerkte JSON naar het bestand
        File.WriteAllText(_filepath, updatedJson);
    }

}
