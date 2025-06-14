using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface ILoginRepository
{
    Employee GetUserByEmail(string email);

    // LoginViewModel GetUserById(long id);
    Task<bool> UpdatePassword(Employee employee);
    Task<bool> ExistUserByEmail(string Email);
    Task<bool> RegisterUser(Employee model);
}