using System.Text.Json.Serialization;

public class LocationDTO : IDTO{
    [JsonPropertyName("row")] 
    public required string Row { get; set; }
    [JsonPropertyName("rack")]
    public required string Rack { get; set; }
    [JsonPropertyName("shelf")]
    public required string Shelf { get; set; }
    [JsonPropertyName("warehouse_id")]
    public required Guid WarehouseId { get; set; }


}