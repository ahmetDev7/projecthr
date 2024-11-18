using Microsoft.AspNetCore.Mvc;
using DTO.Supplier;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly SupplierProvider _supplierProvider;

    public SupplierController(SupplierProvider supplierProvider)
    {
        _supplierProvider = supplierProvider;
    }

    [HttpPost]
    public IActionResult Create(SupplierReQuestDTO request)
    {
        try
        {
            Supplier? supplier = _supplierProvider.Create(request);

            if (supplier == null)
                return BadRequest("Could not create supplier.");

            return Ok(supplier);
        }
        catch (ApiFlowException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var supplier = _supplierProvider.GetById(id);

        if (supplier == null)
            return NotFound("Supplier not found.");

        return Ok(supplier);
    }
}
