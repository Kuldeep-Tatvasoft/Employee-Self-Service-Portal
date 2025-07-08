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
}
