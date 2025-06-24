using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
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
                                                    
                                                    return new DashboardViewModel
                                                    {
                                                        TodayOnLeave = todayOnLeave,
                                                        UpcomingEvents = upcomingEvents
                                                    };
    }
}
