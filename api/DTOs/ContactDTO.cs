using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ContactDTO : IDTO
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}