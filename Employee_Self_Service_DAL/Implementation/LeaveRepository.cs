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

    public async Task<LeaveRequestPaginationViewModel> GetPaginatedRequest(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus, int employeeId)
    {

        var query = _context.LeaveRequests
                    .Include(l => l.Status)
                    .Where(l => !l.IsDeleted && l.EmployeeId == employeeId)
                    .Select(l => new LeaveRequestDetailsViewModel
                    {
                        LeaveRequestId = l.LeaveRequestId,
                        StartDate = (DateOnly)l.StartDate,
                        EndDate = (DateOnly)l.EndDate,
                        ActualDuration = (decimal)l.ActualLeaveDuration,
                        TotalDuration = (decimal)l.TotalLeaveDuration,
                        ReturnDate = (DateOnly)l.ReturnDate,
                        ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        ApprovedBy = l.ApprovedByNavigation.Name,
                        AvailableOnPhone = (bool)l.AvailableOnPhone,
                        StatusId = (int)l.StatusId,
                        Status = l.Status.Status,
                        ReasonName = l.ReasonNavigation.Reason1,
                        AdhocLeave = (bool)l.AdhocLeave,
                        Date = DateOnly.FromDateTime(l.CreatedAt.Value)
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.ActualDuration.ToString().Contains(searchQuery)
                                    || c.ReasonName.ToLower().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(leaveRequestFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(leaveRequestFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestToDate))
        {
            DateOnly toDate = DateOnly.Parse(leaveRequestToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestStatus) && !leaveRequestStatus.Equals("1"))
        {
            if (int.TryParse(leaveRequestStatus, out int statusId))
            {
                query = query.Where(x => x.StatusId == statusId);
            }
        }

        query = sortColumn switch
        {
            "StartDate" => sortDirection == "asc" ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate),
            "EndDate" => sortDirection == "asc" ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate),
            "ActualDuration" => sortDirection == "asc" ? query.OrderBy(x => x.ActualDuration) : query.OrderByDescending(x => x.ActualDuration),
            "TotalDuration" => sortDirection == "asc" ? query.OrderBy(x => x.TotalDuration) : query.OrderByDescending(x => x.TotalDuration),
            "ReturnDate" => sortDirection == "asc" ? query.OrderBy(x => x.ReturnDate) : query.OrderByDescending(x => x.ReturnDate),
            "AvailableOnPhone" => sortDirection == "asc" ? query.OrderBy(x => x.AvailableOnPhone) : query.OrderByDescending(x => x.AvailableOnPhone),
            "ApprovedDate" => sortDirection == "asc" ? query.OrderBy(x => x.ApprovedDate) : query.OrderByDescending(x => x.ApprovedDate),
            "ApprovedBy" => sortDirection == "asc" ? query.OrderBy(x => x.ApprovedBy) : query.OrderByDescending(x => x.ApprovedBy),
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

    public async Task<byte []> GetLeaveRequestToExcel (int pageSize, int pageNumber, string searchQuery,string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {

        var query = _context.LeaveRequests
                    .Include(l => l.Status)
                    .Where(l => !l.IsDeleted && l.EmployeeId == employeeId)
                    .OrderBy(l => l.LeaveRequestId)
                    .Select(l => new LeaveRequestDetailsViewModel
                    {
                        LeaveRequestId = l.LeaveRequestId,
                        StartDate = (DateOnly)l.StartDate,
                        EndDate = (DateOnly)l.EndDate,
                        ActualDuration = (decimal)l.ActualLeaveDuration,
                        TotalDuration = (decimal)l.TotalLeaveDuration,
                        ReturnDate = (DateOnly)l.ReturnDate,
                        ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        ApprovedBy = l.ApprovedByNavigation.Name,
                        StatusId = (int)l.StatusId,
                        Status = l.Status.Status,
                        ReasonName = l.ReasonNavigation.Reason1
                    });
        
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.ActualDuration.ToString().Contains(searchQuery)
                                    || c.ReasonName.ToLower().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(leaveRequestFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(leaveRequestFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestToDate))
        {
            DateOnly toDate = DateOnly.Parse(leaveRequestToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestStatus) && !leaveRequestStatus.Equals("All"))
        {
            
            query = query.Where(x => x.Status == leaveRequestStatus);
            
        }

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    
                    .ToListAsync();
       
        
        List<LeaveRequestDetailsViewModel> model = list;
        
        
        var excelExporter = new Excel.ExportExcel();
        return excelExporter.ExportToExcel(model, "LeaveRequest",string.IsNullOrEmpty(leaveRequestStatus) ? "All" : leaveRequestStatus,searchQuery);
    }



    public async Task<List<Reason>> GetReason()
    {
        return await _context.Reasons.ToListAsync();
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
            return new ResponseViewModel
            {
                success = true,
                message = "Request Added Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur Add Leave Request:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> AddNotification(Notification addNotification)
    {
        try
        {
            addNotification.CategoryId = (int?)((await _context.LeaveRequests.OrderByDescending(e => e.LeaveRequestId).FirstOrDefaultAsync())?.LeaveRequestId);
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
    public async Task<ResponseViewModel> AddResponseNotification(Notification addNotification, int employeeId)
    {
        try
        {
            addNotification.CategoryId = (int?)((await _context.LeaveRequests.OrderByDescending(e => e.LeaveRequestId).FirstOrDefaultAsync())?.LeaveRequestId);
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
    public async Task<LeaveRequest> GetDetails(int requestId)
    {
        LeaveRequest? leaveRequest = await _context.LeaveRequests.Include(u => u.ReasonNavigation)
                                            .Include(u => u.Employee)
                                            .Include(u => u.Status)
                                            .Include(u => u.ApprovedByNavigation)
                                            .Include(u => u.StartLeaveTypeNavigation)
                                            .Include(u => u.EndLeaveTypeNavigation)
                                            .FirstOrDefaultAsync(u => u.LeaveRequestId == requestId);
        return leaveRequest ?? new LeaveRequest();
    }

    public async Task<ResponseViewModel> EditRequest(LeaveRequest request)
    {
        try
        {
            _context.LeaveRequests.Update(request);
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
                message = ex.Message
            };
        }
    }

    public async Task<LeaveRequestPaginationViewModel> GetPaginatedResponse(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus, int employeeId)
    {
        var query = _context.LeaveRequests
                    .Include(l => l.Employee)
                    .Include(l => l.Status)
                    .Where(l => !l.IsDeleted && l.EmployeeId != employeeId)
                    .Select(l => new LeaveRequestDetailsViewModel
                    {
                        LeaveRequestId = l.LeaveRequestId,
                        EmployeeId = l.EmployeeId,
                        EmployeeEmail = l.Employee.Email,
                        EmployeeName = l.Employee.Name,
                        StartDate = (DateOnly)l.StartDate,
                        EndDate = (DateOnly)l.EndDate,
                        ActualDuration = (decimal)l.ActualLeaveDuration,
                        TotalDuration = (decimal)l.TotalLeaveDuration,
                        ReturnDate = (DateOnly)l.ReturnDate,
                        ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        ApprovedBy = l.ApprovedByNavigation.Name,
                        AvailableOnPhone = (bool)l.AvailableOnPhone,
                        StatusId = (int)l.StatusId,
                        Status = l.Status.Status,
                        ReasonName = l.ReasonNavigation.Reason1,
                        AdhocLeave = (bool)l.AdhocLeave,
                        Date = DateOnly.FromDateTime(l.CreatedAt.Value)
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.ActualDuration.ToString().Contains(searchQuery)
                                    || c.ReasonName.ToLower().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(leaveRequestFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(leaveRequestFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestToDate))
        {
            DateOnly toDate = DateOnly.Parse(leaveRequestToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestStatus) && !leaveRequestStatus.Equals("1"))
        {
            if (int.TryParse(leaveRequestStatus, out int statusId))
            {
                query = query.Where(x => x.StatusId == statusId);
            }
        }

        query = sortColumn switch
        {
            "StartDate" => sortDirection == "asc" ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate),
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

    public async Task<byte []> GetResponseDataToExport (int pageSize, int pageNumber, string searchQuery,string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {
        var query = _context.LeaveRequests
                    .Include(l => l.Status)
                    .Where(l => !l.IsDeleted && l.EmployeeId != employeeId)
                    .OrderBy(l => l.LeaveRequestId)
                    .Select(l => new LeaveRequestDetailsViewModel
                    {
                        LeaveRequestId = l.LeaveRequestId,
                        EmployeeName = l.Employee.Name,
                        StartDate = (DateOnly)l.StartDate,
                        EndDate = (DateOnly)l.EndDate,
                        ActualDuration = (decimal)l.ActualLeaveDuration,
                        TotalDuration = (decimal)l.TotalLeaveDuration,
                        ReturnDate = (DateOnly)l.ReturnDate,
                        ApprovedDate = l.ApprovedAt.HasValue ? DateOnly.FromDateTime(l.ApprovedAt.Value).ToString("yyyy-MM-dd") : string.Empty,
                        ApprovedBy = l.ApprovedByNavigation.Name,
                        StatusId = (int)l.StatusId,
                        Status = l.Status.Status,
                        ReasonName = l.ReasonNavigation.Reason1
                    });
                
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.ActualDuration.ToString().Contains(searchQuery)
                                    || c.ReasonName.ToLower().Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(leaveRequestFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(leaveRequestFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestToDate))
        {
            DateOnly toDate = DateOnly.Parse(leaveRequestToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }

        if (!string.IsNullOrEmpty(leaveRequestStatus) && !leaveRequestStatus.Equals("All"))
        {
            
            query = query.Where(x => x.Status == leaveRequestStatus);
            
        }

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    
                    .ToListAsync();
        

        List<LeaveRequestDetailsViewModel> model = list;
    
         var excelExporter = new Excel.ExportExcel();
         return excelExporter.ExportToExcel(list, "LeaveRequest",string.IsNullOrEmpty(leaveRequestStatus) ? "All" : leaveRequestStatus,searchQuery);
    }

}
