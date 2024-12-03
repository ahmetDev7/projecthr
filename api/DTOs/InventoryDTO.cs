using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;


[ApiExplorerSettings(IgnoreApi = true)]
public class InventoryRequest : BaseDTO
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("item_reference")]
    public string? ItemReference { get; set; }

    [JsonPropertyName("item_id")]
    public Guid? ItemId { get; set; }

    [JsonPropertyName("locations")]
    public List<InventoryLocation>? Locations { get; set; }
}


[ApiExplorerSettings(IgnoreApi = true)]
public class InventoryResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("item_reference")]
    public string? ItemReference { get; set; }

    [JsonPropertyName("item_id")]
    public Guid? ItemId { get; set; }

    [JsonPropertyName("locations")]
    public List<InventoryLocation>? Locations { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("total_on_hand")]
    public int? TotalOnHand { get; set; }

    [JsonPropertyName("total_expected")]
    public int? TotalExpected { get; set; }

    [JsonPropertyName("total_ordered")]
    public int? TotalOrdered { get; set; }

    [JsonPropertyName("total_allocated")]
    public int? TotalAllocated { get; set; }

    [JsonPropertyName("total_available")]
    public int? TotalAvailable { get; set; }
}


public class InventoryLocation : BaseDTO
{
    [JsonPropertyName("location_id")]
    public Guid? LocationId { get; set; }

    [JsonPropertyName("on_hand")]
    public int? OnHand { get; set; }
}

