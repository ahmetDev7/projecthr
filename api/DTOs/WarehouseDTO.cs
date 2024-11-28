using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DTO.Address;

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
    public AddressRequest? Address { get; set; }
}



