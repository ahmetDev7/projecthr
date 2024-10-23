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
        List<Warehouse>? decodedLocations = JsonSerializer.Deserialize<List<Warehouse>>(jsonString, options);

        return decodedLocations;
    }

   public Warehouse GetByID(int id)
    {
        var jsonString = File.ReadAllText(_filepath);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true
        };

        // Deserialiseer de JSON naar een lijst van warehouses
        List<Warehouse> warehouses = JsonSerializer.Deserialize<List<Warehouse>>(jsonString, options);

        // Zoek naar een warehouse met het gegeven id
        var warehouse = warehouses.FirstOrDefault(w => w.id == id);

        // Controleer of een warehouse is gevonden
        if (warehouse == null)
        {
            throw new InvalidOperationException($"Warehouse with id {id} does not exist!");
        }

        return warehouse;
    }

}
