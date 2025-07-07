// using System.Text.RegularExpressions;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class HelpDeskRepository : IHelpDeskRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public HelpDeskRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<List<Group>> GetGroups()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task<List<GroupCategory>> GetCategories(int groupId)
    {
        return await _context.GroupCategories.Where(u => u.GroupId == groupId).ToListAsync();
    }

    public async Task<List<SubCategory>> GetSubCategories(int categoryId)
    {
        return await _context.SubCategories.Where(u => u.CategoryId == categoryId).ToListAsync();
    }
}
