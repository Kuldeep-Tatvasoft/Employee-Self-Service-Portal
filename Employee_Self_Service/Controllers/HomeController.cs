using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Employee_Self_Service.Models;
using Employee_Self_Service.Authorization;
using Employee_Self_Service_BAL.Interface;

namespace Employee_Self_Service.Controllers;

public class HomeController : Controller
{
    private readonly ILeaveService _leaveService;
    private readonly IDashboardService _dashboardService;

    public HomeController(ILeaveService leaveService, IDashboardService dashboardService)
    {
        _leaveService = leaveService;
        _dashboardService = dashboardService;
    }

    
    public async Task<IActionResult> Index()
    {   
        var dashboardData = await _dashboardService.GetDashboardData();
        return View(dashboardData);
    }



    [CustomAuthorize ("Employee")]
    public IActionResult Privacy()
    {
        return View();
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
