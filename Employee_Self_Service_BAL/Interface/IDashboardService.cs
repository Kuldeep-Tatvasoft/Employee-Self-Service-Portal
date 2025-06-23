using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardData();
}
