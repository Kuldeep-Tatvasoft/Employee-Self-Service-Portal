using System.Text.RegularExpressions;
using Employee_Self_Service.Hubs;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Constants;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;

namespace Employee_Self_Service.Controllers;

public class HelpDeskController : Controller
{
    private readonly IHelpDeskService _helpDeskService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public HelpDeskController(IHelpDeskService helpDeskService,IHubContext<NotificationHub> hubContext)
    {
        _helpDeskService = helpDeskService;
        _hubContext = hubContext;
    }

    public IActionResult HelpDeskRequest()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetRequestList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, string leaveRequestStatus, int employeeId)
    {
        var model = await _helpDeskService.GetRequestData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, helpDeskRequestGroup, helpDeskRequestStatus, employeeId);
        return PartialView("_HelpDeskRequestListPartialView", model);
    }

    [HttpGet]
    public async Task<IActionResult> AddEditHelpdeskRequest(long requestId)
    {
        AddHelpDeskRequestViewModel model = new AddHelpDeskRequestViewModel
        {
            RequestedDate = DateTime.Now
        };
        if (requestId > 0)
        {
            model = await _helpDeskService.GetEditDetails(requestId);
            ViewBag.Groups = new SelectList(await _helpDeskService.GetGroups(), "GroupId", "GroupName", model.GroupId);
            ViewBag.Categories = new SelectList(await _helpDeskService.GetCategories(model.GroupId), "CategoryId", "CategoryName", model.CategoryId);
            ViewBag.SubCategories = new SelectList(await _helpDeskService.GetSubCategories(model.CategoryId), "SubCategoryId", "SubCategoryName", model.SubCategoryId);
        }
        // else
        // {
        //     ViewBag.Groups = new SelectList(await _helpDeskService.GetGroups(), "GroupId", "GroupName");
        //     ViewBag.Categories = new SelectList(Enumerable.Empty<SelectListItem>());
        //     ViewBag.SubCategories = new SelectList(Enumerable.Empty<SelectListItem>());
        // }

        ViewBag.PriorityList = Enum.GetValues(typeof(Priority))
            .Cast<Priority>()
            .Select(p => new SelectListItem
            {
                Value = p.ToString(), // Use enum name for binding
                Text = p.ToString()
            })
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _helpDeskService.GetGroups();
        return Json(groups.Select(g => new { value = g.GroupId, text = g.GroupName }));
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(int groupId)
    {
        var categories = await _helpDeskService.GetCategories(groupId);
        return Json(categories.Select(c => new { value = c.CategoryId, text = c.CategoryName }));
    }

    [HttpGet]
    public async Task<IActionResult> GetSubCategories(int categoryId)
    {
        var subCategories = await _helpDeskService.GetSubCategories(categoryId);
        return Json(subCategories.Select(sc => new { value = sc.SubCategoryId, text = sc.SubCategoryName }));
    }

    [HttpPost]
    public async Task<IActionResult> AddEditHelpDeskRequest([FromForm] AddHelpDeskRequestViewModel model)
    {

        ResponseViewModel response;
        if (model.HelpDeskRequestId == 0)
        {
            response = await _helpDeskService.AddRequest(model);
        }
        else
        {
            response = await _helpDeskService.EditRequest(model);
        }
        
        if (response.success)
        {   
            
            string notificationMessage = $"Add HelpDesk Request: {model.EmployeeName} Help of {model.Group}";
            response = await _helpDeskService.AddNotification(notificationMessage);
            await _hubContext.Clients.Group("Role_3").SendAsync("ReceiveNotification", notificationMessage);

            TempData["successToastr"] = response.message;
            return RedirectToAction("HelpDeskRequest");
        }
    
        else
        {
            TempData["errorToastr"] = response.message;
            return RedirectToAction("AddEditHelpdeskRequest");

        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteHelpDeskRequest(long helpDeskRequestId)
    {
        ResponseViewModel response = await _helpDeskService.DeleteHelpDeskRequest(helpDeskRequestId);
        if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }
        return RedirectToAction("HelpDeskRequest");
    }
}



