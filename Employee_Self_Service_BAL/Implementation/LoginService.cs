using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_BAL.Implementation;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;

    private readonly IJwtService _jwtService;
    // private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginService(ILoginRepository loginRepository, IJwtService jwtService)
    {
        _loginRepository = loginRepository;
        _jwtService = jwtService;
        // _httpContextAccessor = httpContextAccessor;
    }
    

    #region Login
    // public LoginViewModel GetUserById(long employeeId)
    // {   
    //     return _loginRepository.GetUserById(employeeId);
        
    // }

    public Employee GetUserByEmail(string email)
    {
        return _loginRepository.GetUserByEmail(email);
    }

    public async Task<ResponseViewModel> Login(HttpContext httpContext, LoginViewModel model)
    {   
        try{
            var user = _loginRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                
                return new ResponseViewModel
                {
                    message = "Email is not valid"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!isPasswordValid)
            {
                return new ResponseViewModel{
                    message = "Password is not valid"
                };
            }

            if (user.IsActive == false)
            {
                return new ResponseViewModel{
                    message = "User is not active"
                }; 
            } 
            if(model.RememberMe)   
            {
                CookieOptions option = new CookieOptions(); 
                option.Expires = DateTime.Now.AddHours(24);
                httpContext.Response.Cookies.Append("email", model.Email, option);
            }

             var token = _jwtService.GenerateJwtToken(model.Email,24, user.Role.Role1);
            
            httpContext.Response.Cookies.Append("token", token);
            httpContext.Response.Cookies.Append("role", user.Role.Role1);
            httpContext.Response.Cookies.Append("profileImage", user.ProfileImage ?? "/images/Default_pfp.svg.png");
            httpContext.Response.Cookies.Append("employeeName", user.Name);
            
            return new ResponseViewModel{
                message = "Login Successfully"
            } ;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ResponseViewModel{
                message = "Internal error."
            };
        }
    }
    #endregion
    
    
    public async Task<bool> UpdatePassword(Employee employee)
    {
       return await _loginRepository.UpdatePassword(employee);
    }
    
    public async Task<bool> ExistUserByEmail(string Email)
    {
        try
        {
            return await _loginRepository.ExistUserByEmail(Email);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }
    }
    public async Task<bool> RegisterUser (RegisterViewModel model)
    {
        try
        {   String hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); 
            Employee employee = new Employee
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashPassword,
                RoleId = model.RoleId,
                Department = model.Department,
                Designation = model.Designation,
                Phone = long.TryParse(model.Phone, out var phone) ? phone : 0
            };
            if(model.ProfileImage != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                employee.ProfileImage = $"/uploads/{fileName}";
            }

            return await _loginRepository.RegisterUser(employee);
            
            
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    } 
}