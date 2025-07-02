using Microsoft.AspNetCore.SignalR;

namespace Employee_Self_Service.Hubs;

public class NotificationHub : Hub
{
    // public async Task SendNotificationToOthers(string connectionId, string message)
    // {
    //     await Clients.AllExcept(connectionId).SendAsync("ReceiveNotification", message);
    // }

    public override async Task OnConnectedAsync()
    {
        
        var roleId = Context.User.FindFirst("RoleId")?.Value;
        if (roleId == "3")
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, "Role_3");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        
        var roleId = Context.User.FindFirst("RoleId")?.Value;
        if (roleId == "3")
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Role_3");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotificationToRole3(string message)
    {
        
        await Clients.Group("Role_3").SendAsync("ReceiveNotification", message);
    }
}

