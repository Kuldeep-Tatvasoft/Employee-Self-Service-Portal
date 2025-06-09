using System.Security.Claims;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class LoginController : Controller
{   
    private readonly ILoginService _loginService;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService,  IJwtService jwtService, IEmailService emailService)
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
    public IActionResult Index(LoginViewModel model)
    {   
        
        LoginViewModel? employee =  _loginService.Login(model);

        if (employee.Email == "Email is not valid")
        {   
            TempData["errorToastr"] = employee.Email;
            return View("Index");
        }
        else if(employee.Password == "Password is not valid")
        {
            TempData["errorToastr"] = employee.Password;
            return View("Index");
        }
        else 
        {   
            
            if(model.RememberMe)
            {
                CookieOptions option = new CookieOptions(); 
                option.Expires = DateTime.Now.AddHours(24);
                Response.Cookies.Append("email", model.Email, option);
            }
            
            var token = _jwtService.GenerateJwtToken(model.Email,24, employee.Role);
            Response.Cookies.Append("token", token);
            Response.Cookies.Append("role", employee.Role);
            Response.Cookies.Append("profileImage", employee.ProfileImage ?? "/images/Default_pfp.svg.png");
            Response.Cookies.Append("employeeName", employee.EmployeeName);

            TempData["successToastr"] = "login successfully";

            return RedirectToAction("Index", "Home");
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
    public async Task<IActionResult> SendMail(LoginViewModel model)
    {   
        var user = _loginService.GetUserByEmail(model.Email);
        if(user == null)
        {
            TempData["errorToastr"] = "Email not found";
            return RedirectToAction("ForgotPassword");
        }

        var token = _jwtService.GenerateJwtToken(model.Email,1, " ");
        
        string resetLink = Url.Action("ResetPassword", "Login", new { token }, Request.Scheme);

        var textBody = $@"<div style='max-width: 500px; font-family: Arial, sans-serif; border: 1px solid #ddd;'>
                <div style='background: #006CAC; padding: 10px; text-align: center; height:90px; max-width:100%; display: flex; justify-content: center; align-items: center;'>
                    <img src='https://images.vexels.com/media/users/3/128437/isolated/preview/2dd809b7c15968cb7cc577b2cb49c84f-pizza-food-restaurant-logo.png' style='max-width: 50px;' />
                    <span style='color: #fff; font-size: 24px; margin-left: 10px; font-weight: 600;'>Employee Self Service</span>
                </div>
                <div style='padding: 20px 5px; background-color: #e8e8e8;'>
                    <p>Employee Self Service,</p>
                    <p>Please click <a href='{resetLink}' style='color: #1a73e8; text-decoration: none; font-weight: bold;'>here</a>
                        to reset your account password.</p>
                    <p><strong style='color: orange;'>Important Note:</strong> For security reasons, the link will expire in 24 hours.
                        If you did not request a password reset, please ignore this email or contact our support team immediately.
                    </p>
                </div>
                </div>";
        
        await _emailService.SendEmailAsync(model.Email, "Password Reset Request", textBody);
        TempData["successToastr"] = "Email send successfully";
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
        String hashPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password); 
        employee.Password = hashPassword;
        bool confirm = await _loginService.UpdatePassword(employee);   
        if (confirm)
        {
            TempData["successToastr"] = "Password Reset Successfully";
            return RedirectToAction("Index");
        }
        return RedirectToAction("ResetPassword", new { email = model.Email });
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
        var Email = await _loginService.ExistUserByEmail(model.Email!);
        if (Email)
        {
            ModelState.AddModelError("Email", "User with same email already exists.");
        }
        if(!ModelState.IsValid)
        {
            return View(model);
        }
       
        var isRegister = await _loginService.RegisterUser(model);
        if (isRegister)
        {    
            TempData["successToastr"] = "Registration successfully";      
            var textBody = $@"<div style='max-width: 500px; font-family: Arial, sans-serif; border: 1px solid #ddd;'>
                <div style='background: #006CAC; padding: 10px; text-align: center; height:90px; max-width:100%; display: flex; justify-content: center; align-items: center;'>
                    
                    <span style='color: #fff; font-size: 24px; margin-left: 10px; font-weight: 600;'>Employee Self Service</span>
                </div>
                <div style='padding: 20px 5px; background-color: #e8e8e8;'>
                    <p>Welcome to Employee Self Service</p>
                    <p>Please find the details below for login into your account.</p>
                    <p>Login Details:</p> 
                    <p>Username: {model.Email}</p>
                    
                    
                </div>
                </div>";
            await _emailService.SendEmailAsync(model.Email,"User Credential", textBody);     
            return RedirectToAction("Index", "Login");
        }
        else
        {   
            TempData["errorToastr"] = "Registration Failed, Please try again";
            return View(model);
        }
        
    }
    #endregion
    public IActionResult Logout()
    {
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("token");
        Response.Cookies.Delete("role");
        return RedirectToAction("Index", "Login");
    }
}  