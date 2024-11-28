using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DTO;

[ApiExplorerSettings(IgnoreApi = true)]
public class WarehouseRequest : BaseDTO
{
    [Required]
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("contact_id")]
    public Guid? ContactId { get; set; }

    [JsonPropertyName("contact")]
    public ContactDTO? Contact { get; set; }

    [JsonPropertyName("address_id")]
    public Guid? AddressId { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }
}

public class WarehouseResponse : BaseDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("contact")]
    public ContactDTO? Contact { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}



