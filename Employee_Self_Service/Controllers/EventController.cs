using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class EventController : Controller
{   
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public IActionResult Events()
    {
        return View();
    }
    public async Task<IActionResult> GetEventList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory)
    {   
        var model = await _eventService.GetEventData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, eventFromDate, eventToDate, eventCategory);
        return PartialView("_EventList", model);
    }
    public IActionResult AddEvent()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromForm] AddEventViewModel model)
    {   
       

        ResponseViewModel response = await _eventService.AddEvent(model);
        if (response.success)
        {
            TempData["successToastr"] = response.message;
            return Json(new { success = true });
        }
        else
        {
            TempData["errorToastr"] = response.message;
            return Json(new { success = false});
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> EventDetails(long eventId)
    {
        var model = await _eventService.EventDetails(eventId);
        return PartialView("_EventDetailsPartialView", model);
    }


}
