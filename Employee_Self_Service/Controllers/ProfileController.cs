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
    public ProfileController(IProfileService profileService, IJwtService jwtService)
    {
        _profileService = profileService;
        _jwtService = jwtService;
    }
    public async Task<IActionResult> MyProfile()
    {   
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var details = await _profileService.GetUserDetails(email);
        if (details == null)
        {
            TempData["errorToastr"] = "User details not found.";
            return RedirectToAction("Index", "Home");
        }
        return View(details);
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var token = _jwtService.ValidateToken(Request.Cookies["token"]);
        var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var details = await _profileService.GetUserDetails(email);
        if (details == null)
        {
            TempData["errorToastr"] = "User details not found.";
            return RedirectToAction("Index", "Home");
        }
        return View(details);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(ProfileViewModel model)
    {
        // Fetch the existing profile data
        // var existingProfile = await _profileService.GetUserDetails(model.Email);
        // if (existingProfile == null)
        // {
        //     TempData["errorToastr"] = "Profile not found.";
        //     return RedirectToAction("MyProfile");
        // }

        // // Preserve existing values for blank fields
        // // existingProfile.DateOfBirth = model.DateOfBirth ?? existingProfile.DateOfBirth;
        // existingProfile.BloodGroup = string.IsNullOrWhiteSpace(model.BloodGroup) ? existingProfile.BloodGroup : model.BloodGroup;
        // existingProfile.AnyDiseases = string.IsNullOrWhiteSpace(model.AnyDiseases) ? existingProfile.AnyDiseases : model.AnyDiseases;
        // existingProfile.ContactNo = string.IsNullOrWhiteSpace(model.ContactNo) ? existingProfile.ContactNo : model.ContactNo;

        // Save the updated profile
        var isEdit = await _profileService.UpdateProfile(model);
        if (isEdit)
        {
            TempData["successToastr"] = "Profile updated successfully.";
        }
        else
        {
            TempData["errorToastr"] = "Failed to update profile.";
            return RedirectToAction("EditProfile");
        }
        return RedirectToAction("MyProfile");

    }

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
        if(email == null)
        {
            TempData["errorToastr"] = "Email Not Found";
        }
        model.Email = email;

        ResponseViewModel response = await _profileService.ChangePassword(model);
        if (response.success)
        {
            TempData["successToastr"] = response.message;
        }
        else
        {
            TempData["errorToastr"] = response.message;
            return View("ChangePassword", model);
        }
        return RedirectToAction("Index", "Login");
    }
}
