using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface ILoginService
{
    LoginViewModel Login(LoginViewModel model);
    Employee GetUserByEmail(string email);
    Task<bool> UpdatePassword(Employee employee);
    Task<bool> RegisterUser (RegisterViewModel model);
}
