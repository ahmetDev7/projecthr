using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs;

public class WarehouseDTO : IDTO
{
    [Required]
    public required string Code { get; set; }

    [Required]
    public required string Name { get; set; }

    [JsonPropertyName("contact_id")]
    public Guid? ContactId { get; set; }

    [JsonPropertyName("contact")]
    public ContactDTO? Contact { get; set; }

    [JsonPropertyName("address_id")]
    public Guid? AddressId { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }
}

public class WarehouseUpdateDTO : IDTO
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    [JsonPropertyName("contact")]
    public ContactDTO? Contact { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }
}

