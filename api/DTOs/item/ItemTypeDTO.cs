using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.ItemType
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemTypeRequest : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemTypeResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}