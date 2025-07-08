using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Constants;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Employee_Self_Service_DAL.Constants.HelpDeskEnum;

namespace Employee_Self_Service.Controllers;

public class HelpDeskController : Controller
{   
    private readonly IHelpDeskService _helpDeskService;

    public HelpDeskController(IHelpDeskService helpDeskService)
    {
        _helpDeskService = helpDeskService;
    }
    public  IActionResult HelpDeskRequest()
    {
        return View();
    }

    public async Task<IActionResult> GetRequestList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, string leaveRequestStatus,int employeeId)
    {   
        var model = await _helpDeskService.GetRequestData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, helpDeskRequestGroup, helpDeskRequestStatus, employeeId);
        return PartialView("_HelpDeskRequestListPartialView", model);
    }

    [HttpGet]
    public async Task<IActionResult> AddEditHelpdeskRequest(int requestId)
    {
        AddHelpDeskRequestViewModel model = new  AddHelpDeskRequestViewModel();
        if(requestId > 0)
        {
            
        }
        ViewBag.PriorityList = Enum.GetValues(typeof(Priority))
            .Cast<Priority>()
            .Select(p => new SelectListItem
            {
                Value = ((int)p).ToString(),
                Text = p.ToString()
            })  
            .ToList();

        return View(model);
        // var groups = await _helpDeskService.GetGroups();
        
        // ViewBag.Group = new SelectList(await _helpDeskService.GetGroups(), "GroupId", "GroupName");
        // ViewBag.Categories = new SelectList(await _helpDeskService.GetCategories(), "CategoryId", "CategoryName");
        // ViewBag.Group = new SelectList(await _helpDeskService.GetSubCategories(), "GroupId", "GroupName");

        // return View(model);
    }

    public async Task<IActionResult> GetGroups()
    {
        var groups = await _helpDeskService.GetGroups();
        return Json(new SelectList(groups, "GroupId", "GroupName"));
    }
   
    public async Task<IActionResult> GetCategories(int groupId)
    {
        var categories = await _helpDeskService.GetCategories(groupId);
        return Json(new SelectList(categories, "CategoryId", "CategoryName"));
    }
    
    public async Task<IActionResult> GetSubCategories(int categoryId)
    {
        var subCategories = await _helpDeskService.GetSubCategories(categoryId);
        return Json(new SelectList(subCategories, "SubCategoryId", "SubCategoryName"));
    }

    [HttpPost]
    public async Task<IActionResult> AddEditHelpdeskRequest(AddHelpDeskRequestViewModel model)
    {
        ResponseViewModel response;
        if(model.HelpDeskRequestId == 0)
        {
            response = await _helpDeskService.AddRequest(model);
            if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }
        }
        else
        {
            // response = await _helpDeskService.EditRequest(model);
        }

        // if(response.success)
        // {
        //     TempData["successToastr"] = response.message;
        // }
        // else
        // {
        //     TempData["errorToastr"] = response.message;
        // }
        return RedirectToAction("HelpDeskRequest");
    }


}


