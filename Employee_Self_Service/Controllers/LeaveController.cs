using System.Security.Claims;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Self_Service.Controllers;

public class LeaveController : Controller
{
    private readonly ILeaveService _leaveService;
    private readonly IProfileService _profileService;
    private readonly IJwtService _jwtService;

    public LeaveController(ILeaveService leaveService,IProfileService profileService,IJwtService jwtService)
    {
        _leaveService = leaveService;
        _profileService = profileService;
        _jwtService = jwtService;
    }
    public IActionResult LeaveRequest()
    {
        return View();
    }

    public async Task<IActionResult> GetRequestList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus)
    {
        var model = await _leaveService.GetRequestData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus);
        ViewBag.pageSize = pageSize;
        
        return PartialView("_LeaveRequestListPartialView", model);
    }

    [HttpGet]
    public async Task<ActionResult> AddLeaveRequest(int requestId)
    {   
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; 
        AddLeaveRequestViewModel model = new AddLeaveRequestViewModel();
        if (requestId > 0)
        {
            model= await _leaveService.GetEditDetails(requestId);
        }
        
        model.Reasons = await _leaveService.GetReason();
        model.profile = await _profileService.GetUserDetails(email);
    
        ViewBag.LeaveType = new SelectList(await _leaveService.GetLeaveType(), "TypeId", "Type");
        ViewBag.Reason = new SelectList(await _leaveService.GetReason(), "ReasonId", "Reason1");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEditLeaveRequest(AddLeaveRequestViewModel model)
    {
        if(model.LeaveRequestId == 0){
            ResponseViewModel response =  await _leaveService.AddRequest(model);
            if (response.success)
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
            ResponseViewModel response = await _leaveService.EditRequest(model);
            if (response.success)
            {
                TempData["successToastr"] = response.message;
            }
            else
            {
                TempData["errorToastr"] = response.message;
            }
        }
        
        return RedirectToAction("LeaveRequest");
    }

    


}
