using System.Security.Claims;

namespace Employee_Self_Service_BAL.Interface;

public interface IJwtService
{
    string GenerateJwtToken( string email,  string role);
    ClaimsPrincipal? ValidateToken(string token);
}
