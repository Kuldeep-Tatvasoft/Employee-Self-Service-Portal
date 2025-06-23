using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardViewModel> GetDashboardData()
    {
        return await _dashboardRepository.GetDashboardData();

    }
}
