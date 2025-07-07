using Employee_Self_Service_DAL.Models;

namespace Employee_Self_Service_BAL.Interface;

public interface IHelpDeskService
{
    Task<List<Group>> GetGroups();
    Task<List<GroupCategory>> GetCategories(int groupId);
    Task<List<SubCategory>> GetSubCategories(int categoryId);
}
