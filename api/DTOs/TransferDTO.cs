using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

[ApiExplorerSettings(IgnoreApi = true)]
public class TransferRequestCreate : BaseDTO
{
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }
    [JsonPropertyName("transfer_from_id")]

    public Guid? TransferFromId { get; set; }
    [JsonPropertyName("transfer_to_id")]

    public Guid? TransferToId { get; set; }

    [JsonPropertyName("items")]
    public List<TransferItemDTO>? Items { get; set; }
}

public class TransferItemDTO()
{
    public Guid? ItemId { get; set; }
    public int? Amount { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class TransferResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("transfer_from_id")]
    public Guid? TransferFromId { get; set; }

    [JsonPropertyName("transfer_to_id")]
    public Guid? TransferToId { get; set; }

    [JsonPropertyName("transfer_status")]
    public string? TransferStatus { get; set; }

    [JsonPropertyName("items")]
    public List<TransferItemDTO>? Items { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}