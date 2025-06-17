using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service_BAL.Implementation;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveService(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    public async Task<LeaveRequestPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string leaveRequestFromDate, string leaveRequestToDate, string leaveRequestStatus)
    {
        try
        {
            var List = await _leaveRepository.GetPaginatedRequest(pageSize, pageNumber, search, sort, sortDirection, leaveRequestFromDate, leaveRequestToDate, leaveRequestStatus);
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
            AdhocLeave = model.AdhocLeave
        };
        ResponseViewModel response = await _leaveRepository.AddRequest(request);
        return response;
        }
        catch{
            return new ResponseViewModel{
                success = false,
                message = "Request Failed to Add"
            };
        }
    }

    public async Task<AddLeaveRequestViewModel> GetEditDetails(int requestId)
    {
        LeaveRequest details = await _leaveRepository.GetEditDetails(requestId);
        AddLeaveRequestViewModel model = new AddLeaveRequestViewModel
        {
            LeaveRequestId = details.LeaveRequestId,
            EmployeeId = details.EmployeeId,
            ReasonId = (int)details.Reason,
            ReasonDescription = details.ReasonDescription,
            StartDate = (DateOnly)details.StartDate,
            StartLeaveTypeId = (int)details.StartLeaveType,
            EndDate = (DateOnly)details.EndDate,
            EndLeaveTypeId = (int)details.EndLeaveType,
            ActualDuration = details.ActualLeaveDuration,
            TotalDuration = details.TotalLeaveDuration,
            ReturnDate = details.ReturnDate,
            AlternatePhoneNo = details.AlternatePhoneMo,
            AvailableOnPhone = details.AvailableOnPhone,
            AdhocLeave = (bool)details.AdhocLeave
        };
        return model;   
    }

    public async Task<ResponseViewModel> EditRequest(AddLeaveRequestViewModel model)
    {
        try
        {

        LeaveRequest request = await _leaveRepository.GetEditDetails(model.LeaveRequestId);
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
        };

        ResponseViewModel response = await _leaveRepository.EditRequest(request);
        return response;
        }
        catch{
            return new ResponseViewModel{
                success = false,
                message = "Request Failed to Add"
            };
        }
    }
}
