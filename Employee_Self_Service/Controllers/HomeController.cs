using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Employee_Self_Service.Models;
using Employee_Self_Service.Authorization;
using Employee_Self_Service_BAL.Interface;

namespace Employee_Self_Service.Controllers;

public class HomeController : Controller
{
    private readonly ILeaveService _leaveService;

    public HomeController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    
    public async Task<IActionResult> Index()
    {   
        var todayOnLeave = await _leaveService.GetTodayOnLeave();
        return View(todayOnLeave);
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
