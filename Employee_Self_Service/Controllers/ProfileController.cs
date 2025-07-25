using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Employee_Self_Service.Controllers;

public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly IJwtService _jwtService;
    private readonly ILoginService _loginService;
    public ProfileController(IProfileService profileService, IJwtService jwtService, ILoginService loginService)
    {
        _profileService = profileService;
        _jwtService = jwtService;
        _loginService = loginService;
    }

    #region My Profile
    public async Task<IActionResult> MyProfile()
    {
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            TempData["errorToastr"] = "Email not found.";
            return RedirectToAction("Index", "Login");
        }
        var details = await _profileService.GetUserDetails(email);
        if (details == null)
        {
            TempData["errorToastr"] = "User details not found.";
            return RedirectToAction("Index", "Home");
        }
        return View(details);
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile(int employeeId)
    {
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            TempData["errorToastr"] = "Email not found.";
            return RedirectToAction("Index", "Login");
        }
        var details = await _profileService.GetUserDetails(email);
        if (details == null)
        {
            TempData["errorToastr"] = "User details not found.";
            return RedirectToAction("Index", "Home");
        }
        return View(details);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile([FromForm]ProfileViewModel model)
    {
        ResponseViewModel response = await _profileService.UpdateProfile(model,HttpContext);
        if (response.success)
        {
            TempData["successToastr"] = response.message;
            
            return RedirectToAction("MyProfile");
        }
        else
        {
            TempData["errorToastr"] = response.message;
            return RedirectToAction("EditProfile");
        }
    }
    #endregion

    #region  Change Password
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(LoginViewModel model)
    {
        if (model.NewPassword == model.Password)
        {
            TempData["errorToastr"] = "Current Password and new password not be same";
            return View("ChangePassword", model);
        }
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            TempData["errorToastr"] = "Email Not Found";
            return RedirectToAction("Index","Login");
        }
        model.Email = email;

        ResponseViewModel response = await _profileService.ChangePassword(model);
        if (response.success)
        {
            TempData["successToastr"] = response.message;

            return RedirectToAction("Index", "Login");
        }
        else
        {
            TempData["errorToastr"] = response.message;
            return View("ChangePassword", model);
        }
    }
    #endregion

    #region WidgetsSettings
    [HttpGet]
    public async Task<IActionResult> WidgetsSettings()
    {   
        var employeeId = int.Parse(Request.Cookies["EmployeeId"]);
        var widgets = await _profileService.GetWidgets(employeeId);       
        return View(widgets);
    }

    [HttpPost]
    public async Task<IActionResult> WidgetsSettings(long widgetId)
    {   
        var employeeId = int.Parse(Request.Cookies["EmployeeId"]);
        ResponseViewModel response = await _profileService.AddRemoveWidget(widgetId,employeeId);
        if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }        
        return Json(new {response.success});       
    }
    #endregion

    #region QuickLinksSettings
    
    public async Task<IActionResult> QuickLinks()
    {   
        var employeeId = int.Parse(Request.Cookies["EmployeeId"]);
        var quickLink = await _profileService.GetQuickLink(employeeId);
        return View(quickLink);
    }

    [HttpPost]
    public async  Task<IActionResult> QuickLinks([FromBody] List<QuickLinkViewModel> links)
    {   
        var employeeId = int.Parse(Request.Cookies["EmployeeId"]);
        ResponseViewModel response = await _profileService.AddQuickLink(links,employeeId);
        if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }  
    
        return Json(new { response.success});
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQuickLink(long quickLinkId)
    {
        ResponseViewModel response = await _profileService.DeleteQuickLink(quickLinkId);
        if(response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
        }

        return RedirectToAction("QuickLinks");
    }
    #endregion
}
