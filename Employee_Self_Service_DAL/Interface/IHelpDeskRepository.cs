using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface IHelpDeskRepository
{
    Task<List<Group>> GetGroups();   
    Task<List<GroupCategory>> GetCategories(int groupId);
    Task<List<SubCategory>> GetSubCategories(int categoryId);
    Task<HelpDeskPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, int employeeId);
    Task<ResponseViewModel> AddRequest(HelpdeskRequest request);
    Task<HelpdeskRequest> GetDetails(long requestId);
    Task<ResponseViewModel> EditRequest(HelpdeskRequest request);
}
