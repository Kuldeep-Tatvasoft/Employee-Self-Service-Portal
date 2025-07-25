using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface ILeaveService
{
    Task<LeaveRequestPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    Task<List<Reason>> GetReason();
    Task<List<LeaveType>> GetLeaveType();
    Task<ResponseViewModel> AddRequest(AddLeaveRequestViewModel model);
    Task<ResponseViewModel> AddNotification(string notification);
    Task<AddLeaveRequestViewModel> GetEditDetails(int requestId);
    Task<ResponseViewModel> EditRequest(AddLeaveRequestViewModel model);
    Task<ResponseViewModel> DeleteLeaveRequest(int requestId);
    Task<LeaveRequestPaginationViewModel> GetResponseData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    Task<ResponseViewModel> ResponseLeaveRequest(int requestId, int statusId, int approvedBy, string comment);
    Task<ResponseViewModel> AddResponseNotification(string notification,int employeeId);
    Task<byte[]> GetLeaveDataToExport(int pageSize, int pageNumber, string search, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    Task<byte[]> GetResponseDataToExport(int pageSize, int pageNumber, string search, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId);
    // Task<List<LeaveRequestDetailsViewModel>> GetTodayOnLeave();
    
}
