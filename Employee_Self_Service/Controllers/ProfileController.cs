using Employee_Self_Service_BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    public async Task<IActionResult> MyProfile()
    {   
        
        return View();
    }
}
