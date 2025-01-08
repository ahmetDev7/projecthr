using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using DTO.Address;
using DTO.Contact;


[ApiExplorerSettings(IgnoreApi = true)]
public class WarehouseRequest : BaseDTO
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("contact_ids")]
    public List<Guid?>? ContactIds { get; set; }

    [JsonPropertyName("address_id")]
    public Guid? AddressId { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class WarehouseResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("contacts")]
    public List<ContactResponse>? Contacts { get; set; }

    [JsonPropertyName("address")]
    public AddressResponse? Address { get; set; }

    [JsonPropertyName("dock")]
    public DockResponse? Dock { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}