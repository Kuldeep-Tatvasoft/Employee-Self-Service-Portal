using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IProfileRepository
{
    Task<Employee> GetUserDetails(string email);
    Task<bool> UpdateProfile(ProfileViewModel model);
    Task<ResponseViewModel> ChangePassword(Employee user);
}
