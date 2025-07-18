// using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Employee_Self_Service_BAL.Helpers;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
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
                    .Include(h => h.StatusHistories)
                    .ThenInclude(st => st.StatusNavigation)
                    .Include(h => h.PendingAtNavigation)
                    .Include(h => h.Group)
                    .Include(h => h.Category)
                    .Include(h => h.SubCategory)
                    .Include(h => h.Status)
                    .Include(h => h.SubcategoryMappings)
                    .Where(h => h.InsertedBy == employeeId && h.DeletedAt == null )
                    .Select(h => new AddHelpDeskRequestViewModel
                    {
                        HelpDeskRequestId = h.HelpdeskRequestId,
                        RequestedDate = (DateTime)h.InsertedAt,
                        Group = h.Group.GroupName,
                        Category = h.Category.CategoryName,
                        // SubCategory = _context.SubcategoryMappings.Where(s => s.RequestId == h.HelpdeskRequestId).Select(s => s.SubCategory.SubCategoryName).FirstOrDefaultAsync(),
                        SubCategories = h.SubcategoryMappings.Where(s => s.RequestId == h.HelpdeskRequestId).Select(s => s.SubCategory.SubCategoryName).ToList(),
                        Priority = (Constants.HelpDeskEnum.Priority)h.Priority,
                        ServiceDetails = h.ServiceDetails,
                        Status = h.Status.StatusName,
                        GroupId = (int)h.GroupId,
                        StatusId = (int)h.StatusId,
                        PendingAt = h.PendingAtNavigation != null ? h.PendingAtNavigation.Role1 : string.Empty,
                        // StatusId = h.PendingAt == 3 ? (int)h.StatusId : (int)h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault(),
                        // historyStatus = h.PendingAt == 3 ? (int)h.StatusId : (int)h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault(),
                        // historyStatus = h.
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
            "PendingAt" => sortDirection == "asc" ? query.OrderBy(x => x.PendingAt) : query.OrderByDescending(x => x.PendingAt),
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
    public async Task<ResponseViewModel> AddRequest(HelpdeskRequest request, int[] selectedSubCategories)
    {
        try
        {
            _context.Add(request);
            await _context.SaveChangesAsync();
            if (selectedSubCategories.Any())
            {
                foreach (var subCategoryId in selectedSubCategories)
                {
                    _context.SubcategoryMappings.Add(new SubcategoryMapping
                    {
                        RequestId = request.HelpdeskRequestId,
                        SubCategoryId = (int)subCategoryId
                    });
                }
                await _context.SaveChangesAsync();
            }
            return new ResponseViewModel
            {
                success = true,
                message = "HelpDesk Request Added Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur Add HelpDesk Request:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> AddNotification(Notification addNotification)
    {
        try
        {
            addNotification.CategoryId = (int?)((await _context.HelpdeskRequests.OrderByDescending(e => e.HelpdeskRequestId).FirstOrDefaultAsync())?.HelpdeskRequestId);
            await _context.AddAsync(addNotification);
            await _context.SaveChangesAsync();

            List<Employee> employee = await _context.Employees.Include(u => u.Role).Where(u => u.RoleId == 3).ToListAsync();
            foreach (var user in employee)
            {
                NotificationMapping mapping = new NotificationMapping
                {
                    NotificationId = addNotification.NotificationId,
                    UserId = user.EmployeeId
                };
                await _context.AddAsync(mapping);
                await _context.SaveChangesAsync();
            }
            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false
            };
        }
    }

    public async Task<HelpdeskRequest> GetDetails(long requestId)
    {
        HelpdeskRequest? helpdeskRequest = await _context.HelpdeskRequests
                                         .Include(h => h.StatusHistories)   
                                         .ThenInclude(st => st.StatusNavigation )
                                         .Include(h => h.InsertedByNavigation)   
                                         .Include(h => h.PendingAtNavigation)                                      
                                         .Include(h => h.Group)
                                         .Include(h => h.Category)
                                         .Include(h => h.Status)
                                         .Include(h => h.SubcategoryMappings)
                                         .ThenInclude(sc => sc.SubCategory)
                                         .Where(h => h.HelpdeskRequestId == requestId)
                                         .FirstOrDefaultAsync();
        return helpdeskRequest ?? new HelpdeskRequest();
    }

    public async Task<ResponseViewModel> EditRequest(HelpdeskRequest helpDeskRequest, int[] selectedSubCategories)
    {
        try
        {
            _context.Update(helpDeskRequest);
            await _context.SaveChangesAsync();


            var existingLinks = _context.SubcategoryMappings
                .Where(l => l.RequestId == helpDeskRequest.HelpdeskRequestId);
            _context.SubcategoryMappings.RemoveRange(existingLinks);
            if (selectedSubCategories.Any())
            {
                foreach (var subCategoryId in selectedSubCategories)
                {
                    _context.SubcategoryMappings.Add(new SubcategoryMapping
                    {
                        RequestId = helpDeskRequest.HelpdeskRequestId,
                        SubCategoryId = (int)subCategoryId
                    });
                }
                await _context.SaveChangesAsync();
            }


            return new ResponseViewModel
            {
                success = true,
                message = "HelpDesk Request Update Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur updating HelpDesk Request: " + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> EditRequest(HelpdeskRequest helpDeskRequest)
    {
        try
        {
            _context.Update(helpDeskRequest);
            await _context.SaveChangesAsync();

            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur cancel HelpDesk Request: " + ex.Message
            };
        }
    }

    public async Task<HelpDeskPaginationViewModel> GetPaginatedResponse(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string helpDeskResponseGroup, string helpDeskResponseStatus, int employeeId, string role)
    {

        var query = _context.HelpdeskRequests
                    .Include(h => h.StatusHistories)
                    .ThenInclude(st => st.StatusNavigation)
                    .Include(h => h.Group)
                    .Include(h => h.Category)
                    .Include(h => h.SubCategory)
                    .Include(h => h.Status)
                    .Include(h => h.SubcategoryMappings)
                    .Where(h => role == "Network" ? (int)h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault() == 2 || (int)h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault() == 1 : h.StatusId != 3 && h.InsertedBy != employeeId && h.StatusId != 3)
                    .Select(h => new AddHelpDeskRequestViewModel
                    {
                        HelpDeskRequestId = h.HelpdeskRequestId,
                        RequestedDate = (DateTime)h.InsertedAt,
                        Group = h.Group.GroupName,
                        Category = h.Category.CategoryName,
                        // SubCategory = _context.SubcategoryMappings.Where(s => s.RequestId == h.HelpdeskRequestId).Select(s => s.SubCategory.SubCategoryName).FirstOrDefaultAsync(),
                        SubCategories = h.SubcategoryMappings.Where(s => s.RequestId == h.HelpdeskRequestId).Select(s => s.SubCategory.SubCategoryName).ToList(),
                        Priority = (Constants.HelpDeskEnum.Priority)h.Priority,
                        ServiceDetails = h.ServiceDetails,
                        Status = h.PendingAt == 3 ? h.Status.StatusName:  h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.StatusNavigation.StatusName).FirstOrDefault(),
                        GroupId = (int)h.GroupId,
                        StatusId = h.PendingAt == 3 ? (int)h.StatusId : (int)h.StatusHistories.Where(s  => s.RequestId == h.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault(),
                        PendingAt = h.PendingAtNavigation != null ? h.PendingAtNavigation.Role1 : string.Empty
                        // ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        // Date = DateOnly.FromDateTime(l.CreatedAt.Value)
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.HelpDeskRequestId.ToString().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(helpDeskResponseGroup) && !helpDeskResponseGroup.Equals("0"))
        {
            if (int.TryParse(helpDeskResponseGroup, out int groupId))
            {
                query = query.Where(x => x.GroupId == groupId);
            }
        }


        if (!string.IsNullOrEmpty(helpDeskResponseStatus) && !helpDeskResponseStatus.Equals("0"))
        {
            if (int.TryParse(helpDeskResponseStatus, out int statusId))
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

    public async Task<ResponseViewModel> AddStatus(StatusHistory status)
    {
        try
        {
            _context.Add(status);
            await _context.SaveChangesAsync();

            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur Add Status History:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> AddNotificationByHr(Notification addNotification)
    {
        try
        {
            addNotification.CategoryId = (int?)((await _context.HelpdeskRequests.OrderByDescending(e => e.HelpdeskRequestId).FirstOrDefaultAsync())?.HelpdeskRequestId);
            await _context.AddAsync(addNotification);
            await _context.SaveChangesAsync();

            List<Employee> employee = await _context.Employees.Include(u => u.Role).Where(u => u.RoleId == 4).ToListAsync();
            foreach (var user in employee)
            {
                NotificationMapping mapping = new NotificationMapping
                {
                    NotificationId = addNotification.NotificationId,
                    UserId = user.EmployeeId

                };
                await _context.AddAsync(mapping);
                await _context.SaveChangesAsync();
            }
            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false
            };
        }
    }

    public async Task<ResponseViewModel> AddResponseNotification(Notification addNotification, int employeeId)
    {
        try
        {
            addNotification.CategoryId = (int?)((await _context.HelpdeskRequests.OrderByDescending(e => e.HelpdeskRequestId).FirstOrDefaultAsync())?.HelpdeskRequestId);
            await _context.AddAsync(addNotification);
            await _context.SaveChangesAsync();
            NotificationMapping mapping = new NotificationMapping
            {
                NotificationId = addNotification.NotificationId,
                UserId = employeeId
            };
            await _context.AddAsync(mapping);
            await _context.SaveChangesAsync();

            return new ResponseViewModel
            {
                success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false

            };
        }
    }

    public async Task<List<StatusHistoryViewModel>> GetStatusHistory(long requestId)
    {
        return await _context.StatusHistories
            .Include(h => h.Request)
            .Include(h => h.StatusNavigation)
            .Include(h => h.UpdatedByNavigation)
            .Where(h => h.RequestId == requestId)
            .OrderBy(h => h.StatusChnageDate)
            .Select(h => new StatusHistoryViewModel
            {
                Name = h.UpdatedByNavigation.Name,
                StatusChangedDate = (DateTime)h.StatusChnageDate,
                Status = h.StatusNavigation.StatusName,
                Comment = h.Comment
            }).ToListAsync();
    }
}
