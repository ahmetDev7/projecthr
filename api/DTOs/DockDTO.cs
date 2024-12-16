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