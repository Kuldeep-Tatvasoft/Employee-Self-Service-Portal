using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class DashboardRepository : IDashboardRepository
{
    private readonly EmployeeSelfServiceContext _context;
    public DashboardRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> GetDashboardData()
    {   
        // var employeeId = 
        var today = DateOnly.FromDateTime(DateTime.Now);
        List<LeaveRequestDetailsViewModel>? todayOnLeave = await _context.LeaveRequests
                                                    .Include(l => l.Employee)
                                                    .Include(l => l.Status)
                                                    .Where(l => !l.IsDeleted && l.StartDate <= today && l.EndDate >= today && l.StatusId == 2)
                                                    .Select(l => new LeaveRequestDetailsViewModel 
                                                    {   
                                                        EmployeeName = l.Employee.Name,
                                                        StartDate = (DateOnly)l.StartDate,
                                                        EndDate = (DateOnly)l.EndDate,
                                                        ActualDuration = (decimal)l.ActualLeaveDuration,
                                                        
                                                    }).ToListAsync();
        
        List<AddEventViewModel>? upcomingEvents = await _context.Events                                                    
                                                    .Where(e => e.EndDate >= today)
                                                    .Select(e => new AddEventViewModel
                                                    {   
                                                        EventId = e.EventId,
                                                        EventName = e.Name,
                                                        StartDate = (DateOnly)e.StartDate,
                                                        EndDate = (DateOnly)e.EndDate,
                                                    }).ToListAsync();
        
        List<LeaveRequestDetailsViewModel>? onLeave = await _context.LeaveRequests
                                                    .Include(l => l.Employee)
                                                    .Include(l => l.Status)
                                                    .Where(l => !l.IsDeleted && l.EndDate >= today  && l.StatusId == 2)
                                                    .Select(l => new LeaveRequestDetailsViewModel 
                                                    {   
                                                        EmployeeName = l.Employee.Name,
                                                        StartDate = (DateOnly)l.StartDate,
                                                        EndDate = (DateOnly)l.EndDate,
                                                        ActualDuration = (decimal)l.ActualLeaveDuration,
                                                    }).ToListAsync();
        List<HelpDeskDetailsViewModel>? ownHelpDeskRequests = await _context.HelpdeskRequests
                                                            .Include(h => h.StatusHistories)
                                                            .ThenInclude(st => st.StatusNavigation)
                                                            .Include(h => h.PendingAtNavigation)
                                                            .Include(h => h.Group)
                                                            .Include(h => h.Category)
                                                            .Include(h => h.SubCategory)
                                                            .Include(h => h.Status)
                                                            .Include(h => h.SubcategoryMappings)
                                                            .Where(h =>  h.DeletedAt == null )
                                                            .OrderBy(h => h.HelpdeskRequestId)
                                                            .Select(h => new HelpDeskDetailsViewModel
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
                                                                EmployeeId = (int)h.InsertedBy
                                                            }).ToListAsync();
        List<Widget> availableWidgets = await _context.Widgets.ToListAsync();

        List<QuickLinkViewModel> quickLinks = await _context.QuickLinks
                                                    .OrderBy(q => q.QuickLinkId)
                                                    .Select(q => new QuickLinkViewModel
                                                    {
                                                        QuickLinkId = q.QuickLinkId,
                                                        Name = q.Name,
                                                        Url = q.Url
                                                    }).ToListAsync();

                                                    return new DashboardViewModel
                                                    {
                                                        TodayOnLeave = todayOnLeave,
                                                        UpcomingEvents = upcomingEvents,
                                                        OnLeave = onLeave,
                                                        OwnHelpDeskRequests = ownHelpDeskRequests,
                                                        Widgets = availableWidgets,
                                                        QuickLinks = quickLinks
                                                    };
    }
}
