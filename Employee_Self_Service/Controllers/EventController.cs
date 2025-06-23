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
    public IActionResult AddEvent()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromForm] AddEventViewModel model)
    {   
        if (!ModelState.IsValid)
        {
            TempData["errorToastr"] = "Please fill all required fields.";
            return View(model);
        }

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
