using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierReQuestDTO : BaseDTO
{
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public Contact? Contact { get; set; }
    public Address? Address { get; set; }
}

[ApiExplorerSettings(IgnoreApi = true)]
public class SupplierResponseDTO : BaseDTO
{
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public Contact? Contact { get; set; }
    public Address? Address { get; set; }
}