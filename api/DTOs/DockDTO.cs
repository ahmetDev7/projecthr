using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

[ApiExplorerSettings(IgnoreApi = true)]
public class DockResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class DockWithItemsResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("capacity")]
    public int? Capacity {get; set;}

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("dock_items")]
    public List<DockItemResponse> DockItems { get; set; }
}

public class DockItemResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("item_id")]
    public Guid? ItemId { get; set; }

    [JsonPropertyName("amount")]
    public int? Amount {get; set;}

    [JsonPropertyName("dock_id")]
    public Guid? DockId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

}