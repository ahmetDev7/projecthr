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

   public Warehouse GetByID(int id)
    {
        var jsonString = File.ReadAllText(_filepath);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true
        };

        // Deserialiseer de JSON naar een lijst van warehouses
        List<Warehouse> ?warehouses = JsonSerializer.Deserialize<List<Warehouse>>(jsonString, options);

        // Zoek naar een warehouse met het gegeven id
        var warehouse = warehouses?.FirstOrDefault(w => w.id == id);

        // Controleer of een warehouse is gevonden
        if (warehouse == null)
        {
            throw new InvalidOperationException($"Warehouse with id {id} does not exist!");
        }

        return warehouse;
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
