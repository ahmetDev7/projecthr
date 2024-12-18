using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.ItemGroup
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemGroupRequest : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemGroupResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}