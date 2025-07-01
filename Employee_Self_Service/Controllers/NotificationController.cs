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

    // var role = Request.Cookies["role"];
    [HttpGet]
    public async Task<IActionResult> GetNotifications(long roleId)
    {   
        
        var notifications = await _notificationService.GetNotifications(roleId); 
       
        return PartialView("_NotificationPartialView", notifications);
    }
    public async Task<IActionResult> MarkRead(long notificationId)
    {
        ResponseViewModel response =  await _notificationService.MarkRead(notificationId);
        if (response.success)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    public async Task<IActionResult> GetNotificationCount(long roleId)
    {
        var count = await _notificationService.GetNotificationCount(roleId);
        return Json(count);
    }
}
