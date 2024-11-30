using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.Contact
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContactRequest : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContactResponse : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

    }
}
