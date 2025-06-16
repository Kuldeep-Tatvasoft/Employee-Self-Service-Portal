using Employee_Self_Service_BAL.Helpers;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class LeaveRepository : ILeaveRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public LeaveRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<LeaveRequestPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus)
    {
        var query = _context.LeaveRequests
                    .Include(l => l.Status)
                    .Where(l => !l.IsDeleted)
                    .Select(l => new LeaveRequestDetailsViewModel 
                    {
                        LeaveRequestId = l.LeaveRequestId,
                        StartDate = (DateOnly)l.StartDate,
                        EndDate = (DateOnly)l.EndDate,
                        ActualDuration = (int)l.ActualLeaveDuration,
                        TotalDuration = (int)l.TotalLeaveDuration,
                        ReturnDate = (DateOnly)l.ReturnDate,
                        AvailableOnPhone = l.AvailableOnPhone,
                        Status = l.Status.Status,
                        AdhocLeave = l.AdhocLeave,
                        Date = DateOnly.FromDateTime(l.CreatedAt.Value) 
                    });


        if(!string.IsNullOrEmpty(leaveRequestFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(leaveRequestFromDate);
            query = query.Where(x => x.Date >= fromDate);
        }

        if(!string.IsNullOrEmpty(leaveRequestToDate))
        {
            DateOnly toDate = DateOnly.Parse(leaveRequestToDate);
            query = query.Where(x => x.Date <= toDate);
        }
        
        if(!string.IsNullOrEmpty(leaveRequestStatus) && !leaveRequestStatus.Equals("3"))
        {
            query = query.Where(x => x.Status == leaveRequestStatus);
        }

        query = sortColumn switch
        {
            "#StartDate" => sortDirection == "asc" ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate),
            "EndDate" => sortDirection == "asc" ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate),
            "ActualDuration" => sortDirection == "asc" ? query.OrderBy(x => x.ActualDuration) : query.OrderByDescending(x => x.ActualDuration),
            "TotalDuration" => sortDirection == "asc" ? query.OrderBy(x => x.TotalDuration) : query.OrderByDescending(x => x.TotalDuration),
            "ReturnDate" => sortDirection == "asc" ? query.OrderBy(x => x.ReturnDate) : query.OrderByDescending(x => x.ReturnDate),
            "AvailableOnPhone" => sortDirection == "asc" ? query.OrderBy(x => x.AvailableOnPhone) : query.OrderByDescending(x => x.AvailableOnPhone),
            "ApprovedDate" => sortDirection == "asc" ? query.OrderBy(x => x.ApprovedDate) : query.OrderByDescending(x => x.ApprovedDate),
            "Status" => sortDirection == "asc" ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.Status),
            "AdhocLeave" => sortDirection == "asc" ? query.OrderBy(x => x.AdhocLeave) : query.OrderByDescending(x => x.AdhocLeave),
            _ => query.OrderBy(x => x.LeaveRequestId)
        };

        var totalRecords = await query.CountAsync();

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        LeaveRequestPaginationViewModel model = new()
        {
            Page = new(),
            RequestList = list
        };
        model.Page.SetPagination(totalRecords, pageSize, pageNumber);

        return model;
    }

    public async Task<List<Reason>> GetReason()
    {
        return _context.Reasons.ToList();
    }

    public async Task<List<LeaveType>> GetLeaveType()
    {
        return _context.LeaveTypes.ToList();
    }

    public async Task<ResponseViewModel> AddRequest(LeaveRequest request)
    {   
        try
        {   
            _context.Add(request);
            await _context.SaveChangesAsync();
            return new ResponseViewModel{
                success = true,
                message = "Request Added Successfully"
            };
        }
        catch (Exception ex)
        {   
            return new ResponseViewModel{
                success = false,
                message = ex.Message
            };
        }
        

    }
    public async Task<LeaveRequest> GetEditDetails(int requestId)
    {
        return await _context.LeaveRequests.FirstOrDefaultAsync(u => u.LeaveRequestId == requestId);
    }

    public async Task<ResponseViewModel> EditRequest(LeaveRequest request)
    {
        LeaveRequest leaveRequest = await _context.LeaveRequests.Where(u => u.LeaveRequestId == request.LeaveRequestId).FirstOrDefaultAsync();

        if(leaveRequest == null)
        {
            return new ResponseViewModel{
                success = false,
                message = "Leave Request Not Exist."
            };
        }

        try
        {
    
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return new ResponseViewModel{
                success = true,
                message = "Leave Request Update Successfully"
            };
        }
        catch(Exception ex)
        {
            return new ResponseViewModel{
                success = false,
                message = ex.Message
            };
        }
    }

}
