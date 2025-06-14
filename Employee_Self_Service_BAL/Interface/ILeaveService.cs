using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface ILeaveService
{
    Task<LeaveRequestPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus);
}
