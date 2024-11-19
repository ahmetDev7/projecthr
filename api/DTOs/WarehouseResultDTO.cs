using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace DTOs;

public class WarehouseResultDTO : IDTO
{
    public Guid Id {get; set;}
    public string? Code { get; set; }

    public string? Name { get; set; }

    [JsonPropertyName("contact")]
    public ContactDTO? Contact { get; set; }

    [JsonPropertyName("address")]
    public AddressDTO? Address { get; set; }
}