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

    [HttpGet("generate-admin-token")]
    public async Task<IActionResult> GenerateAdminToken()
    {
        var token = await _tokenService.GenerateAdminToken();
        return Ok(new { Token = token });
    }

    [HttpGet("admin-only")]
    [Authorize(Roles = "admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new{message = "Authorized."});
    }
}