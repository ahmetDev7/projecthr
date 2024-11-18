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
    public Guid Contact_id{get;set;}
    public ContactDTO? Contact { get; set; }
    public Guid Address_id{get;set;}
    public AddressDTO? Address { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierResponseDTO : BaseDTO
{
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public ContactDTO? Contact { get; set; }
    public AddressDTO? Address { get; set; }
}