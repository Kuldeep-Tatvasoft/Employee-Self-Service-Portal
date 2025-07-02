using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface ILeaveRepository
{
    Task<LeaveRequestPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    Task<List<Reason>> GetReason();
    Task<List<LeaveType>> GetLeaveType();
    Task<ResponseViewModel> AddRequest(LeaveRequest request);
    Task<ResponseViewModel> AddNotification (Notification addNotification);
    Task<LeaveRequest> GetDetails(int requestId);
    Task<ResponseViewModel> EditRequest(LeaveRequest request);
    Task<LeaveRequestPaginationViewModel> GetPaginatedResponse(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    // Task<List<LeaveRequestDetailsViewModel>> GetTodayOnLeave();
    // Task<DashboardViewModel> GetTodayOnLeave();
}
