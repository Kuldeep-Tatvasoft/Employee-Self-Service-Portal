using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service_BAL.Implementation;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveService(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    #region Employee Side Leave
    public async Task<LeaveRequestPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {
        try
        {
            var List = await _leaveRepository.GetPaginatedRequest(pageSize, pageNumber, search, sort, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
            return List;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Reason>> GetReason()
    {
        var data = await _leaveRepository.GetReason();
        return data;
    }

    public async Task<List<LeaveType>> GetLeaveType()
    {
        var data = await _leaveRepository.GetLeaveType();
        return data;
    }

    public async Task<ResponseViewModel> AddRequest(AddLeaveRequestViewModel model)
    {
        try
        {
            LeaveRequest request = new LeaveRequest
            {
                EmployeeId = model.EmployeeId,
                Reason = model.ReasonId,
                ReasonDescription = model.ReasonDescription,
                StartDate = model.StartDate,
                StartLeaveType = model.StartLeaveTypeId,
                EndDate = model.EndDate,
                EndLeaveType = model.EndLeaveTypeId,
                ActualLeaveDuration = model.ActualDuration,
                TotalLeaveDuration = model.TotalDuration,
                ReturnDate = model.ReturnDate,
                AlternatePhoneMo = model.AlternatePhoneNo,
                AvailableOnPhone = model.AvailableOnPhone,
                AdhocLeave = model.AdhocLeave,
                StatusId = model.AdhocLeave ? 2 : 3 
            };
            
            ResponseViewModel response = await _leaveRepository.AddRequest(request);
            return response;
        }
        catch
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Request Failed to Add"
            };
        }
    }

    public async Task<ResponseViewModel> AddNotification(string notification)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 2,

        };
        
        ResponseViewModel response = await _leaveRepository.AddNotification(addNotification);
        return response;
    }

    


    public async Task<AddLeaveRequestViewModel> GetEditDetails(int requestId)
    {
        LeaveRequest details = await _leaveRepository.GetDetails(requestId);
        if (details == null)
        {
            return null;
        }
        AddLeaveRequestViewModel model = new AddLeaveRequestViewModel
        {
            LeaveRequestId = details.LeaveRequestId,
            EmployeeId = details.EmployeeId,
            EmployeeEmail = details.Employee.Email,
            ReasonId = (int)details.Reason,
            ReasonName = details.ReasonNavigation.Reason1,
            ReasonDescription = details.ReasonDescription,
            StartDate = (DateOnly)details.StartDate,
            StartLeaveTypeId = (int)details.StartLeaveType,
            StartLeaveType = details.StartLeaveTypeNavigation.Type,
            EndDate = (DateOnly)details.EndDate,
            EndLeaveType = details.EndLeaveTypeNavigation.Type,
            EndLeaveTypeId = (int)details.EndLeaveType,
            ActualDuration = details.ActualLeaveDuration,
            TotalDuration = details.TotalLeaveDuration,
            ReturnDate = details.ReturnDate,
            RequestedDate = DateOnly.FromDateTime(details.CreatedAt.Value),
            AlternatePhoneNo = details.AlternatePhoneMo,
            AvailableOnPhone = details.AvailableOnPhone,
            AdhocLeave = (bool)details.AdhocLeave,
            
            StatusId = (int)details.StatusId,
            LeaveStatus = details.Status.Status,
            ApprovedBy = details.ApprovedByNavigation?.Name
        };
        
        return model;
    }

    public async Task<ResponseViewModel> EditRequest(AddLeaveRequestViewModel model)
    {
        try
        {
            LeaveRequest request = await _leaveRepository.GetDetails(model.LeaveRequestId);
            {
                request.EmployeeId = model.EmployeeId;
                request.Reason = model.ReasonId;
                request.ReasonDescription = model.ReasonDescription;
                request.StartDate = model.StartDate;
                request.StartLeaveType = model.StartLeaveTypeId;
                request.EndDate = model.EndDate;
                request.EndLeaveType = model.EndLeaveTypeId;
                request.ActualLeaveDuration = model.ActualDuration;
                request.TotalLeaveDuration = model.TotalDuration;
                request.ReturnDate = model.ReturnDate;
                request.AlternatePhoneMo = model.AlternatePhoneNo;
                request.AvailableOnPhone = model.AvailableOnPhone;
                request.AdhocLeave = model.AdhocLeave;
                request.UpdatedAt = DateTime.Now;
            };

            ResponseViewModel response = await _leaveRepository.EditRequest(request);
            return response;
        }
        catch
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Request Failed to Add"
            };
        }
    }

    public async Task<ResponseViewModel> DeleteLeaveRequest(int requestId)
    {
        try
        {
            LeaveRequest request = await _leaveRepository.GetDetails(requestId);
            {
                request.StatusId = 4;
                request.DeletedAt = DateTime.Now;
            };

            ResponseViewModel response = await _leaveRepository.EditRequest(request);
            if (response.success)
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Request Deleted Successfully"
                };
            }
            else
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Error occur deleting request:" + response.message
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Request Failed to Deleted:" + ex.Message
            };
        }

    }
    #endregion

    #region Response Side
    public async Task<LeaveRequestPaginationViewModel> GetResponseData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {
        try
        {
            var List = await _leaveRepository.GetPaginatedResponse(pageSize, pageNumber, search, sort, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
            return List;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<ResponseViewModel> ResponseLeaveRequest(int requestId, int statusId, int approvedBy, string comment)
    {
        try
        {
            LeaveRequest request = await _leaveRepository.GetDetails(requestId);
            {
                request.StatusId = statusId;
                request.ApprovedAt = DateTime.Now;
                request.ApprovedBy = approvedBy;
                request.Comment = comment;
            }

            ResponseViewModel response = await _leaveRepository.EditRequest(request);
            if(response.success)
            {
                return new ResponseViewModel{
                    success = true,
                    message = "Status Changed Successfully"
                };
            }
            else
            {
                return new ResponseViewModel{
                    success = true,
                    message = "Error occur status change " + response.message
                };
            }
        }
        catch(Exception ex)
        {
            return new ResponseViewModel{
                success = false,
                message = "Failed to update status" + ex.Message
            };
        }
    } 

    public async Task<ResponseViewModel> AddResponseNotification(string notification,int employeeId)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 3,

        };
        
        ResponseViewModel response = await _leaveRepository.AddResponseNotification(addNotification,employeeId);
        return response;
    }
    
    public async Task<byte[]> GetLeaveDataToExport(int pageSize, int pageNumber, string search, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {
        return await _leaveRepository.GetLeaveRequestToExcel(pageSize, pageNumber, search, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
    }

    public async Task<byte[]> GetResponseDataToExport(int pageSize, int pageNumber, string search, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus,int employeeId)
    {
        return await _leaveRepository.GetResponseDataToExport(pageSize, pageNumber, search, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus, employeeId);
    }
    #endregion   
}