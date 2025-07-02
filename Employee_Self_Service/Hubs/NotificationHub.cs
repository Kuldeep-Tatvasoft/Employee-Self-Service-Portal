using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Employee_Self_Service.Hubs;

public class NotificationHub : Hub
{   
    public override async Task OnConnectedAsync()
    {
        var roleId = Context.GetHttpContext()?.Request.Cookies["roleId"];
        if (!string.IsNullOrEmpty(roleId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Role_{roleId}");
            Console.WriteLine($"Added ConnectionId: {Context.ConnectionId} to Role_{roleId} group.");
        }

        var employeeId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(employeeId))
        {
            Console.WriteLine($"Employee {employeeId} connected with ConnectionId: {Context.ConnectionId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roleId = Context.GetHttpContext()?.Request.Cookies["roleId"];
        if (!string.IsNullOrEmpty(roleId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Role_{roleId}");
            Console.WriteLine($"Removed ConnectionId: {Context.ConnectionId} from Role_{roleId} group.");
        }

        var employeeId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(employeeId))
        {
            Console.WriteLine($"Employee {employeeId} disconnected.");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotificationToRole3(string message)
    {
        await Clients.Group("Role_3").SendAsync("ReceiveNotification", message);
    }
}


public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        
        var employeeId = connection.GetHttpContext()?.Request.Cookies["EmployeeId"];
        return employeeId ?? connection.User?.FindFirst("EmployeeId")?.Value;
    }
}

