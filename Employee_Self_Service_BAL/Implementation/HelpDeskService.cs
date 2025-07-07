using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Group = Employee_Self_Service_DAL.Models.Group;

namespace Employee_Self_Service_BAL.Implementation;

public class HelpDeskService : IHelpDeskService
{
    private readonly IHelpDeskRepository _helpDeskRepository;

    public HelpDeskService(IHelpDeskRepository helpDeskRepository)
    {
        _helpDeskRepository = helpDeskRepository;
    }

    public async Task<List<Group>> GetGroups()
    {
        return await _helpDeskRepository.GetGroups();
    }

    public async Task<List<GroupCategory>> GetCategories(int groupId)
    {
        return await _helpDeskRepository.GetCategories(groupId);
    }

    public async Task<List<SubCategory>> GetSubCategories(int categoryId)
    {
        return await _helpDeskRepository.GetSubCategories(categoryId);
    }
}
