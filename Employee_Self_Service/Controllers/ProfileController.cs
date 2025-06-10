using System.Security.Claims;
using Employee_Self_Service_BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Employee_Self_Service.Controllers;

public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly IJwtService _jwtService;
    public ProfileController(IProfileService profileService, IJwtService jwtService)
    {
        _profileService = profileService;
        _jwtService = jwtService;
    }
    public async Task<IActionResult> MyProfile()
    {   
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        // var details = await _profileService.GetUserDetails(email);
        // if (details == null)
        // {
        //     TempData["errorToastr"] = "User details not found.";
        //     return RedirectToAction("Index", "Home");
        // }
        return View();
    }

    

}
