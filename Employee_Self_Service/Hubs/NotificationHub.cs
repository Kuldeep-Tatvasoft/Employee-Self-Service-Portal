using System.Collections.Concurrent;
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

        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roleId = Context.GetHttpContext()?.Request.Cookies["roleId"];
        if (!string.IsNullOrEmpty(roleId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Role_{roleId}");
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

