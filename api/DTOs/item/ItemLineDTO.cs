using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.ItemLine
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemLineRequest : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemLineResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}