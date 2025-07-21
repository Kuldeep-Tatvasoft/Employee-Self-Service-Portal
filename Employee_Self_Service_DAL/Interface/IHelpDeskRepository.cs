using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IHelpDeskRepository
{
    Task<List<Group>> GetGroups();   
    Task<List<GroupCategory>> GetCategories(int groupId);
    Task<List<SubCategory>> GetSubCategories(int categoryId);
    Task<HelpDeskPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, int employeeId);
    Task<ResponseViewModel> AddRequest(HelpdeskRequest request, int [] selectedSubCategories);
    Task<ResponseViewModel> AddNotification(Notification addNotification);
    Task<HelpdeskRequest> GetDetails(long requestId);
    Task<ResponseViewModel> EditRequest(HelpdeskRequest helpDeskRequest,int [] selectedSubCategories);
    Task<ResponseViewModel> EditRequest(HelpdeskRequest helpDeskRequest);
    Task<HelpDeskPaginationViewModel> GetPaginatedResponse(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskResponseGroup, string helpDeskResponseStatus, int employeeId,string role);
    Task<ResponseViewModel> AddStatus(StatusHistory status);
    Task<ResponseViewModel> AddNotificationByHr(Notification addNotification);
    Task<ResponseViewModel> AddResponseNotification(Notification addNotification, int employeeId);
    Task<List<StatusHistoryViewModel>> GetStatusHistory(long requestId);
    Task<byte []> GetHelpDeskDataToExport (int pageSize, int pageNumber, string searchQuery, string helpDeskGroup,string helpDeskStatus,int employeeId);
    Task<byte []> GetHelpDeskResponseDataToExport (int pageSize, int pageNumber, string searchQuery, string helpDeskGroup,string helpDeskStatus,int employeeId);
}
