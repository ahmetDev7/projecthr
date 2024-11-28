using System.Text.Json.Serialization;
using DTO.Contact;
using Microsoft.AspNetCore.Mvc;

namespace DTO.Supplier;

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierRequest : BaseDTO
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("contact_id")]
    public Guid? ContactId { get; set; }

    [JsonPropertyName("contact")]
    public ContactRequest? Contact { get; set; }

    [JsonPropertyName("address_id")]
    public Guid? AddressId { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("contact")]
    public ContactResponse? Contact { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}