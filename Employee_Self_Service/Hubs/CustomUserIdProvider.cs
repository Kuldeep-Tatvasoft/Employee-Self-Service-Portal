using Microsoft.AspNetCore.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        
        var employeeId = connection.GetHttpContext()?.Request.Cookies["EmployeeId"];
        return employeeId ?? connection.User?.FindFirst("EmployeeId")?.Value;
    }
}
