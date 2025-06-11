using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class LeaveController : Controller
{
    public IActionResult LeaveRequest()
    {
        return View();
    }
}
