using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

[ApiExplorerSettings(IgnoreApi = true)]
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

[ApiExplorerSettings(IgnoreApi = true)]
public class LocationResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    public string? Row { get; set; }
    [JsonPropertyName("rack")]
    public string? Rack { get; set; }
    [JsonPropertyName("shelf")]
    public string? Shelf { get; set; }
    [JsonPropertyName("warehouse_id")]
    public Guid? WarehouseId { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}