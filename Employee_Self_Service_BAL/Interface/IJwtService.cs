using System.Security.Claims;

namespace Employee_Self_Service_BAL.Interface;

public interface IJwtService
{
    String GenerateJwtToken( string email,int timeToLive,  string role);
    ClaimsPrincipal? ValidateToken(string token);
}
