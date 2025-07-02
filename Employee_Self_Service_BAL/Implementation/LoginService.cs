using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Employee_Self_Service_BAL.Implementation;

public class LoginService : ILoginService
{
    // private readonly ILoginRepository _loginRepository;
    private readonly IEmployeeRepository _employeeRepository;

    private readonly IJwtService _jwtService;

    public LoginService( IJwtService jwtService, IEmployeeRepository employeeRepository)
    {
        // _loginRepository = loginRepository;
        _jwtService = jwtService;
        _employeeRepository = employeeRepository;
    }


    #region Login
    public Employee GetUserByEmail(string email)
    {
        return _employeeRepository.GetUserByEmail(email);
    }

    public async Task<ResponseViewModel> Login(HttpContext httpContext, LoginViewModel model)
    {
        try
        {
            var user = _employeeRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                return new ResponseViewModel
                {
                    success = false,
                    message = "Email is not valid"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!isPasswordValid)
            {
                return new ResponseViewModel
                {   
                    success = false,
                    message = "Password is not valid"
                };
            }

            if (user.IsActive == false)
            {
                return new ResponseViewModel
                {
                    success = false,
                    message = "User is not active"
                };
            }
            if (model.RememberMe)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddHours(24);
                httpContext.Response.Cookies.Append("email", model.Email, option);
            }
            var token = _jwtService.GenerateJwtToken(model.Email,user.EmployeeId,user.Name, 24, user.Role.Role1);
            httpContext.Response.Cookies.Append("token", token);
            httpContext.Response.Cookies.Append("role", user.Role.Role1);
            httpContext.Response.Cookies.Append("roleId", user.RoleId?.ToString());
            httpContext.Response.Cookies.Append("profileImage", user.ProfileImage ?? "/images/Default_pfp.svg.png");
            httpContext.Response.Cookies.Append("employeeName", user.Name);
            httpContext.Response.Cookies.Append("EmployeeId", user.EmployeeId.ToString());
            return new ResponseViewModel
            {   
                success = true,
                message = "Login Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {   
                success = false,
                message = "Internal error." + ex.Message
            };
        }
    }
    #endregion


    public async Task<ResponseViewModel> UpdatePassword(Employee employee)
    {   try{
            String hashPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password); 
            employee.Password = hashPassword;
            ResponseViewModel response =  await _employeeRepository.UpdateEmployee(employee);
            return response;
        }
        catch(Exception ex)
        {
            return new  ResponseViewModel{
                success = false,
                message = "Failed to Update password" + ex.Message
            }; 
        }
        
    }
    public async Task<ResponseViewModel> RegisterUser(RegisterViewModel model)
    {
        try
        {
            String hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
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
            if (model.ProfileImage != null)
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

            ResponseViewModel response =  await _employeeRepository.RegisterEmployee(employee);
            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel{
                success = false,
                message = "Registration Failed, Please try again" + ex.Message
            };
            
        }
    }
}