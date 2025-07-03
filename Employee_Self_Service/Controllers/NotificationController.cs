using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Employee_Self_Service.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(int employeeId)
    {   
        var notifications = await _notificationService.GetNotifications(employeeId); 
        return PartialView("_NotificationPartialView", notifications);
    }
    public async Task<IActionResult> MarkRead(int employeeId, long notificationId)
    {
        ResponseViewModel response =  await _notificationService.MarkRead(employeeId,notificationId);
        if (response.success)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    public async Task<IActionResult> GetNotificationCount(int employeeId)
    {
        var count = await _notificationService.GetNotificationCount(employeeId);
        return Json(count);
    }
}
