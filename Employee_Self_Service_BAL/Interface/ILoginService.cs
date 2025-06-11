using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_BAL.Interface;

public interface ILoginService
{
    Task<ResponseViewModel> Login(HttpContext httpContext, LoginViewModel model);
    Employee GetUserByEmail(string email);
    Task<bool> UpdatePassword(Employee employee);
    Task<bool> ExistUserByEmail(string Email);
    Task<bool> RegisterUser (RegisterViewModel model);
}