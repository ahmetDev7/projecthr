using System.Text.Json.Serialization;

public class LocationRequest : BaseDTO
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

public class LocationResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("row")]
    public string? Row { get; set; }

    [JsonPropertyName("rack")]
    public string? Rack { get; set; }

    [JsonPropertyName("shelf")]
    public string? Shelf { get; set; }

    [JsonPropertyName("warehouse_id")]
    public Guid? WarehouseId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("update_at")]
    public DateTime? UpdatedAt { get; set; }
}
