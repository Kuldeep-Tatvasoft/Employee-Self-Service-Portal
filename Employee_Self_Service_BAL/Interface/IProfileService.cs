using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IProfileService
{
    Task<ProfileViewModel> GetUserDetails(string email);
    Task<ResponseViewModel> UpdateProfile(ProfileViewModel model);
    Task<ResponseViewModel> ChangePassword(LoginViewModel model);
    Task<ProfileViewModel> GetEmployeeById(int employeeId);
}
