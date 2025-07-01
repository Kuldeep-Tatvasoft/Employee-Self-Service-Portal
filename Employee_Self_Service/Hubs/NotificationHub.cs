using Microsoft.AspNetCore.SignalR;

namespace Employee_Self_Service.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToOthers(string connectionId, string message)
    {
        await Clients.AllExcept(connectionId).SendAsync("ReceiveNotification", message);
    }

    
}
