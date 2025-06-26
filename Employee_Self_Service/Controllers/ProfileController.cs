using System.Security.Claims;
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
}
