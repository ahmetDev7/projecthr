using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DTO.Supplier;

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierReQuestDTO : BaseDTO
{
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public Guid? ContactId { get; set; }
    public ContactDTO? Contact { get; set; }
    public Guid? AddressId { get; set; }
    public AddressDTO? Address { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierResponseDTO : BaseDTO
{
    public Guid Id {get; set;}
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    [JsonIgnore]
    public Contact? ContactId { get; set; }
    public ContactDTO? Contact { get; set; }
    [JsonIgnore]
    public Address? AddressId { get; set; }
    public AddressDTO? Address { get; set; }
}