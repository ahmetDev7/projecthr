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

    public void DeleteWarehouse(int id)
    {
        // 1. Haal de bestaande lijst van warehouses op
        List<Warehouse> warehouses = Getall();

        // 2. Zoek het warehouse op basis van het Id
        var warehouseToRemove = warehouses.FirstOrDefault(w => w.id == id);

        if (warehouseToRemove == null)
        {
            throw new InvalidOperationException($"Warehouse with Id {id} does not exist.");
        }

        // 3. Verwijder het warehouse uit de lijst
        warehouses.Remove(warehouseToRemove);
        


        // 4. Serialize de bijgewerkte lijst terug naar JSON
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