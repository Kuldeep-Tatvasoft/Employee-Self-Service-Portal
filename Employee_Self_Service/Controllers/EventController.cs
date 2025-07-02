using Employee_Self_Service.Authorization;
using Employee_Self_Service.Hubs;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Employee_Self_Service.Controllers;

public class EventController : Controller
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IEventService _eventService;

    public EventController(IEventService eventService, IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
        _eventService = eventService;
    }

    [CustomAuthorize("HR")]
    public IActionResult Events()
    {
        return View();
    }

    public async Task<IActionResult> GetEventList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory)
    {
        var model = await _eventService.GetEventData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, eventFromDate, eventToDate, eventCategory);
        return PartialView("_EventList", model);
    }

    public async Task<IActionResult> AddEditEvent(long eventId)
    {
        AddEventViewModel model = new AddEventViewModel();
        if (eventId > 0)
        {
            model = await _eventService.EventDetails(eventId);
        }
        model.Categories = await _eventService.GetCategories();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEditEvent([FromForm] AddEventViewModel model)
    {
        ResponseViewModel response;
        if (model.EventId == 0)
        {
            response = await _eventService.AddEvent(model);
        }
        else
        {
            response = await _eventService.EditEvent(model);
        }


        var role = Request.Cookies["role"];
        // var connectionId = Request.Cookies["EmployeeId"];
        // var connectionId = long.Parse(employeeId);
        var connectionId = HttpContext.Connection.Id;
        
       if (role == "HR")
        {
            string notificationMessage = $"New Event: {model.EventName} Added  starting on {model.StartDate.ToString("dd/MM/yyyy")}";
            response = await _eventService.AddNotification(notificationMessage);
            await _hubContext.Clients.AllExcept(connectionId).SendAsync("ReceiveNotification", notificationMessage);
        }

        if (response.success)
        {
            TempData["successToastr"] = response.message;
            return Json(new { success = true });
        }
        else
        {
            TempData["errorToastr"] = response.message;
            return Json(new { success = false });
        }
    }

    [HttpGet]
    public async Task<IActionResult> EventDetails(long eventId)
    {
        var model = await _eventService.EventDetails(eventId);
        return PartialView("_EventDetailsPartialView", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEvent(long eventId)
    {
        ResponseViewModel response = await _eventService.DeleteEvent(eventId);
        if (response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }
        return RedirectToAction("Events");
    }    
}