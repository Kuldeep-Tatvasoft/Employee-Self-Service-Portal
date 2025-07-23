using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IEmployeeRepository
{
    Employee GetUserByEmail(string email);
    Employee GetEmployeeById(int employeeId);
    Task<ResponseViewModel> UpdateEmployee(Employee employee);
    Task<ResponseViewModel> RegisterEmployee(Employee employee);
    Task<List<Widget>> GetWidgets();
    Task<ResponseViewModel> UpdateWidget(Widget widget);
    Task<List<QuickLink>> GetQuickLinks();
    Task<List<QuickLinkViewModel>> GetQuickLink();
    Task<ResponseViewModel> AddQuickLink(List<QuickLinkViewModel> links);
}
