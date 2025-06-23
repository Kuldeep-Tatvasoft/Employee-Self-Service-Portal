using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IDashboardRepository 
{
    Task<DashboardViewModel> GetDashboardData();
}
