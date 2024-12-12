using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService {
    public async Task<string> GenerateToken(string role)
    {
        var tokenKey = Encoding.UTF8.GetBytes("SuperSecretKeyThatIs32BytesLongX");
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, role)
        });

        var token = new JwtSecurityToken(
            claims: claims.Claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        );

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateAdminToken()
    {
        return await GenerateToken("admin");
    }

    public async Task<string> GenerateWarehouseManagerToken()
    {
        return await GenerateToken("warehousemanager");
    }

    public async Task<string> GenerateInventoryManagerToken()
    {
        return await GenerateToken("inventorymanager");
    }
    public async Task<string> GenerateFloorManagerToken()
    {
        return await GenerateToken("floormanager");
    }
    public async Task<string> GenerateOperativeToken()
    {
        return await GenerateToken("operative");
    }
    public async Task<string> GenerateSupervisorToken()
    {
        return await GenerateToken("supervisor");
    }
    public async Task<string> GenerateAnalystToken()
    {
        return await GenerateToken("analyst");
    }
    public async Task<string> GenerateLogisticsToken()
    {
        return await GenerateToken("logistics");
    }
    public async Task<string> GenerateSalesToken()
    {
        return await GenerateToken("sales");
    }
}

