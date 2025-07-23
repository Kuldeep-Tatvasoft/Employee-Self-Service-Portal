using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_BAL.Interface;

public interface IProfileService
{
    Task<ProfileViewModel> GetUserDetails(string email);
    Task<ResponseViewModel> UpdateProfile(ProfileViewModel model, HttpContext httpContext);
    Task<ResponseViewModel> ChangePassword(LoginViewModel model);
    Task<ProfileViewModel> GetEmployeeById(int employeeId);
    Task<List<Widget>> GetWidgets();
    Task<ResponseViewModel> AddRemoveWidget(long widgetId);
    // Task<List<QuickLink>> GetQuickLink();
    Task<List<QuickLinkViewModel>> GetQuickLink();
    Task<ResponseViewModel> AddQuickLink(List<QuickLinkViewModel> links);
}
