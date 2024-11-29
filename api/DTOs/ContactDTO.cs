using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ContactDTO : BaseDTO
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [Required]
    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [Required]
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}