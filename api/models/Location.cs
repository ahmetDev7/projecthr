using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Models.Location;

public record LocationDTO : IDTO
{
    [JsonPropertyName("row")]
    public string? Row { get; set; }

    [JsonPropertyName("rack")]
    public string? Rack { get; set; }

    [JsonPropertyName("shelf")]
    public string? Shelf { get; set; }

    [JsonPropertyName("warehouse_id")]
    public Guid? WarehouseId { get; set; }
}


public class Location
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Row { get; set; }
    public required string Rack { get; set; }
    public required string Shelf { get; set; }
    // Foreign Key Relationship
    public required Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}

