using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;
public record LocationResultDTO : IDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("row")]
    public string? Row { get; set; }

    [JsonPropertyName("rack")]
    public string? Rack { get; set; }

    [JsonPropertyName("shelf")]
    public string? Shelf { get; set; }

    [JsonPropertyName("warehouse_id")]
    public Guid? WarehouseId { get; set; }
}