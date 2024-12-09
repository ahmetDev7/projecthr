using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("generate-token")]
    public async Task<IActionResult> GenerateToken([FromQuery] string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return BadRequest(new { Error = "Role must be specified." });
        }

        var validRoles = new[] { "admin", "warehousemanager", "inventorymanager", "floormanager", "operative", "supervisor", "analyst", "logistics", "sales" };
        if (!validRoles.Contains(role.ToLower()))
        {
            return BadRequest(new { Error = $"Role '{role}' is not recognized." });
        }

        var token = await _tokenService.GenerateToken(role.ToLower());
        return Ok(new { Token = token });
    }
    
    [HttpGet("admin-only")]
    [Authorize(Roles = "admin,operative")]
    public IActionResult AdminOnly()
    {
        return Ok(new{message = "Admin Only Authorized."});
    }
}