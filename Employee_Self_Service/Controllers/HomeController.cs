using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Employee_Self_Service.Models;
using Employee_Self_Service.Authorization;
using Employee_Self_Service_BAL.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [CustomAuthorize("Employee", "HR", "Network")]
    public async Task<IActionResult> Index()
    {
        var dashboardData = await _dashboardService.GetDashboardData();
        return View(dashboardData);
    }

    [HttpGet]
    public IActionResult CheckUrl(string url)
    {
        try
        {
            var fullUrl = new Uri($"{Request.Scheme}://{Request.Host}{url}").AbsoluteUri;
            var client = new HttpClient();
            var response = client.GetAsync(fullUrl).Result;
            return Json(new { exists = response.IsSuccessStatusCode, redirectUrl = response.IsSuccessStatusCode ? url : "/Home/Error404" });
        }
        catch
        {
            return Json(new { exists = false, redirectUrl = "/Home/Error404" });
        }
    }

    public IActionResult Error404()
    {
        return View();
    }

    [CustomAuthorize("Employee")]
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
