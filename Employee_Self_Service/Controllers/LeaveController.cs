using Employee_Self_Service_BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class LeaveController : Controller
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
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

    
    public IActionResult AddLeaveRequest()
    {
        return View();
    }

    // public async Task<IActionResult> AddLeaveRequest()
    // {

    // }
}
