using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.ItemGroup
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemGroupRequest : IDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemGroupResponse : IDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}