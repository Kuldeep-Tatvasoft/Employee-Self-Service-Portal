using System.Security.Claims;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service.Controllers;

public class LoginController : Controller
{   
    private readonly ILoginService _loginService;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public LoginController(ILoginService loginService,  IJwtService jwtService, IEmailService emailService)
    {
       _loginService = loginService;
       _jwtService = jwtService;
       _emailService = emailService;
    }

    #region Login
    public IActionResult Index()
    {   
        if(Request.Cookies.ContainsKey("email"))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {   
        
        ResponseViewModel? response = await _loginService.Login(HttpContext,model);

        if (response.success)
        {   
            TempData["successToastr"] = response.message;
            return RedirectToAction("Index", "Home");
        }
       else 
        {  
            TempData["errorToastr"] = response.message;
            return View("Index"); 
           
        }      
    } 

    public IActionResult LoginRedirect()
    {
        return View();
    }
    #endregion
    
    #region Forgot Password
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(LoginViewModel model)
    {   
        var user = _loginService.GetUserByEmail(model.Email);
        if(user == null)
        {
            TempData["errorToastr"] = "Email not found";
            return RedirectToAction("ForgotPassword");
        }

        var token = _jwtService.GenerateJwtToken(model.Email,user.EmployeeId,user.Name,1, " ");
        
        string resetLink = Url.Action("ResetPassword", "Login", new { token }, Request.Scheme);       
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "Mail.html");
        string htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);
        htmlBody = htmlBody.Replace("{resetLink}", resetLink);
        ResponseViewModel response = await _emailService.SendEmailAsync(model.Email, "Password Reset Request", htmlBody);
        if(response.success){
            TempData["successToastr"] = response.message;
        }
        else{
            TempData["errorToastr"] = response.message;
        }
        return RedirectToAction("Index");
    }
    #endregion
    
    #region Reset Password
    public IActionResult ResetPassword(string token)
    {
        var principal = _jwtService.ValidateToken(token);
        var email = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            TempData["errorToastr"] = "Link is Expired try again";
            return RedirectToAction("Index");
        }
        LoginViewModel model = new LoginViewModel();
        model.Email = email;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(LoginViewModel model)
    {
        var employee = _loginService.GetUserByEmail(model.Email);
        if (employee == null)
        {
            TempData["errorToastr"] = "Email not found";
            return View(model);
        }
        employee.Password = model.NewPassword;        
        ResponseViewModel response = await _loginService.UpdatePassword(employee);   
        if (response.success)
        {
            TempData["successToastr"] = response.message;
            return RedirectToAction("Index");
        }
        else{
            TempData["errorToastr"] = response.message;
            return RedirectToAction("ResetPassword", new { email = model.Email });
        }
        
    }
    #endregion
    
    #region Registration
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register (RegisterViewModel model)
    {   
        var employee = _loginService.GetUserByEmail(model.Email);
        if (employee != null)
        {
            ModelState.AddModelError("Email", "User with same email already exists.");
        }
        if(!ModelState.IsValid)
        {
            return View(model);
        }
       
        ResponseViewModel response = await _loginService.RegisterUser(model);
        if (response.success)
        {    
            TempData["successToastr"] = response.message;      
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "Registration.html");
            string htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);  
            htmlBody = htmlBody.Replace("{Email}", model.Email);  
            await _emailService.SendEmailAsync(model.Email,"User Credential", htmlBody);     
           
            return RedirectToAction("Index", "Login");
        }
        else
        {   
            TempData["errorToastr"] = response.message;
            return View(model);
        }
    }
    #endregion

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("token");
        Response.Cookies.Delete("role");
        return RedirectToAction("Index", "Login");
    }
}  