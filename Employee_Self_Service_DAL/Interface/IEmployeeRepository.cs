using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IEmployeeRepository
{
    Employee GetUserByEmail(string email);
    Task<ResponseViewModel> UpdateEmployee(Employee employee);
    Task<ResponseViewModel> RegisterUser(Employee employee);
    Task<ResponseViewModel> UpdateProfile(Employee employee);
    Task<ResponseViewModel> ChangePassword(Employee employee);
}
