using System.Security.Claims;
using Employee_Self_Service.Authorization;
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

    #region Employee Side Leave 
    public IActionResult LeaveRequest()
    {
        return View();
    }
    
    public async Task<IActionResult> GetRequestList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {   
        var model = await _leaveService.GetRequestData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
        return PartialView("_LeaveRequestListPartialView", model);
    }
    [HttpGet]
    public async Task<ActionResult> AddEditLeaveRequest(int requestId)
    {   
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
           TempData["errorToastr"] = "Email not found.";
           return RedirectToAction("Index", "Home");
        } 
        AddLeaveRequestViewModel model = new AddLeaveRequestViewModel();
        if (requestId > 0)
        {
            model= await _leaveService.GetEditDetails(requestId);
        }
        
        model.Reasons = await _leaveService.GetReason();
        model.profile = await _profileService.GetUserDetails(email);
    
        ViewBag.LeaveType = new SelectList(await _leaveService.GetLeaveType(), "TypeId", "Type");
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

    [HttpPost]
    public async Task<IActionResult> DeleteLeaveRequest(int leaveRequestId)
    {
        ResponseViewModel response = await _leaveService.DeleteLeaveRequest(leaveRequestId);
        if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }
        return RedirectToAction("LeaveRequest");
    }

    [HttpGet]
    public async Task<IActionResult> LeaveView(int leaveRequestId)
    {   
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
           TempData["errorToastr"] = "Email not found.";
           return RedirectToAction("Index", "Home");
        }
        var model = await _leaveService.GetEditDetails(leaveRequestId);
        model.profile = await _profileService.GetUserDetails(email);        
        return PartialView("_LeaveViewPartialView", model);
    }
    
    #endregion

    #region Response  
    [CustomAuthorize ("HR")]
    public IActionResult ResponseRequest()
    {
        return View();
    }

    public async Task<IActionResult> GetResponseList(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {   
        var model = await _leaveService.GetResponseData(pageSize, pageNumber, searchQuery, sortColumn, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
        return PartialView("_ResponseLeaveRequestList", model);
    }

    [HttpGet]
    public async Task<IActionResult> ResponseLeaveRequest(int requestId, string email)
    {
        AddLeaveRequestViewModel model = new AddLeaveRequestViewModel();
        model= await _leaveService.GetEditDetails(requestId);
        model.Reasons = await _leaveService.GetReason();
        model.profile = await _profileService.GetUserDetails(email);
    
        ViewBag.LeaveType = new SelectList(await _leaveService.GetLeaveType(), "TypeId", "Type");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResponseLeaveRequest(int requestId, int statusId, int approvedBy, string comment)
    {
        ResponseViewModel response = await _leaveService.ResponseLeaveRequest(requestId, statusId, approvedBy,comment);
        if(response.success)
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
    #endregion
}