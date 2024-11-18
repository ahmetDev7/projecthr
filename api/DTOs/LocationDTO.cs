using System.Text.Json.Serialization;

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
