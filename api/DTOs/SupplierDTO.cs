using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DTO.Supplier;

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierRequest : BaseDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public Guid? Contact_id { get; set; }
    public ContactDTO? Contact { get; set; }
    public Guid? Address_id { get; set; }
    public AddressDTO? Address { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierResponse : BaseDTO
{
    public Guid Id {get; set;}
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public ContactDTO? Contact { get; set; }
    public AddressDTO? Address { get; set; }
    public string? Created_at{get;set;}
    public string? Updated_at{get;set;}
}