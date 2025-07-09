// using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Helpers;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
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

    public async Task<HelpDeskPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, int employeeId)
    {

        var query = _context.HelpdeskRequests
                    .Include(h => h.Group)
                    .Include(h => h.Category)
                    .Include(h => h.SubCategory)
                    .Include(h => h.Status)
                    .Where(h => h.InsertedBy == employeeId)
                    .Select(h => new AddHelpDeskRequestViewModel
                    {
                        HelpDeskRequestId = h.HelpdeskRequestId,
                        RequestedDate = (DateTime)h.InsertedAt,
                        Group = h.Group.GroupName,
                        Category = h.Category.CategoryName,
                        SubCategory = h.SubCategory.SubCategoryName,
                        Priority = (Constants.HelpDeskEnum.Priority)h.Priority,
                        ServiceDetails = h.ServiceDetails,
                        Status = h.Status.StatusName,
                        GroupId = (int)h.GroupId,
                        StatusId = (int)h.StatusId,
                        
                        // ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        // Date = DateOnly.FromDateTime(l.CreatedAt.Value)
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.HelpDeskRequestId.ToString().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(helpDeskRequestGroup) && !helpDeskRequestGroup.Equals("0"))
        {
            if (int.TryParse(helpDeskRequestGroup, out int groupId))
            {
                query = query.Where(x => x.GroupId == groupId);
            }
        }
        

        if (!string.IsNullOrEmpty(helpDeskRequestStatus) && !helpDeskRequestStatus.Equals("0"))
        {
            if (int.TryParse(helpDeskRequestStatus, out int statusId))
            {
                query = query.Where(x => x.StatusId == statusId);
            }
        }


        query = sortColumn switch
        {
            "HelpDeskRequestId" => sortDirection == "asc" ? query.OrderBy(x => x.HelpDeskRequestId) : query.OrderByDescending(x => x.HelpDeskRequestId),
            "RequestedDate" => sortDirection == "asc" ? query.OrderBy(x => x.RequestedDate) : query.OrderByDescending(x => x.RequestedDate),
            "Group" => sortDirection == "asc" ? query.OrderBy(x => x.Group) : query.OrderByDescending(x => x.Group),
            "Category" => sortDirection == "asc" ? query.OrderBy(x => x.Category) : query.OrderByDescending(x => x.Category),
            "SubCategory" => sortDirection == "asc" ? query.OrderBy(x => x.SubCategory) : query.OrderByDescending(x => x.SubCategory),
            "Priority" => sortDirection == "asc" ? query.OrderBy(x => x.Priority) : query.OrderByDescending(x => x.Priority),
            "ServiceDetails" => sortDirection == "asc" ? query.OrderBy(x => x.ServiceDetails) : query.OrderByDescending(x => x.ServiceDetails),
            "Status" => sortDirection == "asc" ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.Status),
            _ => query.OrderBy(x => x.HelpDeskRequestId)
        };

        var totalRecords = await query.CountAsync();

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        HelpDeskPaginationViewModel model = new()
        {
            Page = new(),
            HelpDeskList = list
        };
        model.Page.SetPagination(totalRecords, pageSize, pageNumber);

        return model;
    }
    public async Task<ResponseViewModel> AddRequest(HelpdeskRequest request)
    {
        try
        {
            _context.Add(request);
            await _context.SaveChangesAsync();
            return new ResponseViewModel
            {
                success = true,
                message = "HelpDesk Request Added Successfully"
            };
        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur Add HelpDesk Request:" + ex.Message
            };
        }
    }

    public async Task<HelpdeskRequest> GetDetails(long requestId)
    {
       HelpdeskRequest? helpdeskRequest = await _context.HelpdeskRequests
                                        .Include(h => h.Group)
                                        .Include(h => h.Category)
                                        .Include(h => h.SubCategory)
                                        .Include(h => h.Status)
                                        .Where(h => h.HelpdeskRequestId == requestId)
                                        .FirstOrDefaultAsync();  
        return helpdeskRequest ?? new HelpdeskRequest();
    }

    public async Task<ResponseViewModel> EditRequest(HelpdeskRequest request)
    {
        try
        {
            _context.Update(request);
            await _context.SaveChangesAsync();
            return new ResponseViewModel{
                success = true,
                message = "HelpDesk Request Update Successfully"
            };
        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur updating HelpDesk Request: " +ex.Message
            };
        }
    }
}
