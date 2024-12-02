using DTO.Address;
using DTO.Contact;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.Client
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClientRequest : BaseDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("contact_id")]
        public Guid? ContactId { get; set; }

        [JsonPropertyName("contact")]
        public ContactRequest? Contact { get; set; }

        [JsonPropertyName("address_id")]
        public Guid? AddressId { get; set; }

        [JsonPropertyName("address")]
        public AddressRequest? Address { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClientResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("contact")]
        public ContactResponse? Contact { get; set; }

        [JsonPropertyName("address")]
        public AddressResponse? Address { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
