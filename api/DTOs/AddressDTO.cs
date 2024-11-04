using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class AddressDTO : IDTO
{
    [Required]
    [JsonPropertyName("street")]
    public required string Street { get; set; }

    [Required]
    [JsonPropertyName("house_number")]
    public required string HouseNumber { get; set; }
    
    [JsonPropertyName("house_number_extension")]
    public string? HouseNumberExtension { get; set; }

    [JsonPropertyName("house_number_extension_extra")]
    public string? HouseNumberExtensionExtra { get; set; }

    [Required]
    [JsonPropertyName("zipcode")]
    public required string ZipCode { get; set; }
    
    [Required]
    [JsonPropertyName("city")]
    public required string City { get; set; }
    
    [JsonPropertyName("province")]
    public string? Province { get; set; }

    [Required]
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; set; }
}
