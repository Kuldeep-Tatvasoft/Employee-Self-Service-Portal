using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IEmployeeRepository
{
    Employee GetUserByEmail(string email);
    Employee GetEmployeeById(int employeeId);
    Task<ResponseViewModel> UpdateEmployee(Employee employee);
    Task<ResponseViewModel> RegisterEmployee(Employee employee);
    Task<List<WidgetMapping>> GetWidgets(int employeeId);
    Task<ResponseViewModel> UpdateWidget(WidgetMapping widget);
    Task<List<QuickLink>> GetQuickLinks();
    Task<List<QuickLinkViewModel>> GetQuickLink(long employeeId);
    Task<ResponseViewModel> AddQuickLink(List<QuickLinkViewModel> links,int employeeId);
    Task<ResponseViewModel> UpdateQuickLink(QuickLink quickLink);
}
