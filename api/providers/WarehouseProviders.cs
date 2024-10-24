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

    public void UpdateWarehouse(Warehouse updatedWarehouse)
    {
        // 1. Haal de bestaande lijst van warehouses op
        List<Warehouse> warehouses = Getall();

        // 2. Zoek naar het bestaande warehouse op basis van het Id
        var existingWarehouseIndex = warehouses.FindIndex(w => w.id == updatedWarehouse.id);

        if (existingWarehouseIndex == -1)
        {
            throw new InvalidOperationException($"Warehouse with Id {updatedWarehouse.id} does not exist.");
        }

        // 3. Vervang het bestaande warehouse met het nieuwe warehouse
        warehouses[existingWarehouseIndex] = updatedWarehouse;

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