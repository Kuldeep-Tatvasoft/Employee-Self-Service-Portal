using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IHelpDeskService
{
    Task<List<Group>> GetGroups();
    Task<List<GroupCategory>> GetCategories(int groupId);
    Task<List<SubCategory>> GetSubCategories(int categoryId);
    Task<HelpDeskPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus,int employeeId);
    Task<ResponseViewModel> AddRequest(AddHelpDeskRequestViewModel model);
    Task<ResponseViewModel> AddNotification(string notification);
    Task<AddHelpDeskRequestViewModel> GetEditDetails(long requestId);
    Task<ResponseViewModel> EditRequest(AddHelpDeskRequestViewModel model);
    Task<ResponseViewModel> DeleteHelpDeskRequest(long requestId);
    Task<HelpDeskPaginationViewModel> GetResponseData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string helpDeskResponseGroup, string helpDeskResponseStatus, int employeeId,string role);
    Task<ResponseViewModel> ResponseHelpDeskRequest(AddHelpDeskRequestViewModel model);
    Task<ResponseViewModel> AddNotificationByHr(string notification);
    Task<ResponseViewModel> AddResponseNotification(string notification,int employeeId);
    Task<List<StatusHistoryViewModel>> GetStatusHistory(long requestId);
}
