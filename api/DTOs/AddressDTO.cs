using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.Address
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AddressRequest : BaseDTO
    {
        [JsonPropertyName("street")]
        public string? Street { get; set; }
        
        [JsonPropertyName("house_number")]
        public string? HouseNumber { get; set; }
        
        [JsonPropertyName("house_number_extension")]
        public string? HouseNumberExtension { get; set; }
        
        [JsonPropertyName("house_number_extension_extra")]
        public string? HouseNumberExtensionExtra { get; set; }
        
        [JsonPropertyName("zipcode")]
        public string? ZipCode { get; set; }
        
        [JsonPropertyName("city")]
        public string? City { get; set; }
        
        [JsonPropertyName("province")]
        public string? Province { get; set; }
        
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class AddressResponse : BaseDTO
    {        
        [JsonPropertyName("street")]
        public string? Street { get; set; }
        
        [JsonPropertyName("house_number")]
        public string? HouseNumber { get; set; }
        
        [JsonPropertyName("house_number_extension")]
        public string? HouseNumberExtension { get; set; }
        
        [JsonPropertyName("house_number_extension_extra")]
        public string? HouseNumberExtensionExtra { get; set; }
        
        [JsonPropertyName("zipcode")]
        public string? ZipCode { get; set; }
        
        [JsonPropertyName("city")]
        public string? City { get; set; }
        
        [JsonPropertyName("province")]
        public string? Province { get; set; }
        
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
    }
}
