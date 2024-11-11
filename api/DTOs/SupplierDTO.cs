using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs;
public class SupplierDTO : IDTO
{
    [Required]
    public required string? code {get;set;}

    [Required]
    public required string? name {get;set;}

    [JsonPropertyName("reference")]
    public string? reference {get;set;}

    [JsonPropertyName("contact_id")]
    public required Guid? contact_id{get;set;}

    [JsonPropertyName("address_id")]
    public required Guid? address_id{get;set;}
    
}