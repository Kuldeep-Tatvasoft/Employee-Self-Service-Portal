using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveService(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    public async Task<LeaveRequestPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus)
    {
        try
        {
            var List = await _leaveRepository.GetPaginatedRequest(pageSize, pageNumber, search, sort, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus);
            return List;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
}
