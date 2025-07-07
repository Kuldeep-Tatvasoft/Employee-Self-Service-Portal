using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    [HttpGet]
    public async Task<IActionResult> AddEditHelpdeskRequest(int requestId)
    {
        AddHelpDeskRequestViewModel model = new  AddHelpDeskRequestViewModel();
        if(requestId > 0)
        {
            
        }

        // var groups = await _helpDeskService.GetGroups();
        
        // ViewBag.Group = new SelectList(await _helpDeskService.GetGroups(), "GroupId", "GroupName");
        // ViewBag.Categories = new SelectList(await _helpDeskService.GetCategories(), "CategoryId", "CategoryName");
        // ViewBag.Group = new SelectList(await _helpDeskService.GetSubCategories(), "GroupId", "GroupName");

        return View();
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
}


