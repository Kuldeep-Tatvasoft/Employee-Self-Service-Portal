using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IProfileService
{
    Task<ProfileViewModel> GetUserDetails(string email);
    Task<bool> UpdateProfile(ProfileViewModel model);
    Task<ResponseViewModel> ChangePassword(LoginViewModel model);
}
