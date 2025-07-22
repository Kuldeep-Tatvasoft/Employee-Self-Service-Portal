using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Constants;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Group = Employee_Self_Service_DAL.Models.Group;

namespace Employee_Self_Service_BAL.Implementation;

public class HelpDeskService : IHelpDeskService
{
    private readonly IHelpDeskRepository _helpDeskRepository;

    public HelpDeskService(IHelpDeskRepository helpDeskRepository)
    {
        _helpDeskRepository = helpDeskRepository;
    }

    #region HelpDesk Request
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
    public async Task<HelpDeskPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus, int employeeId)
    {
        try
        {
            return await _helpDeskRepository.GetPaginatedRequest(pageSize, pageNumber, search, sort, sortDirection, helpDeskRequestGroup, helpDeskRequestStatus, employeeId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<ResponseViewModel> AddRequest(AddHelpDeskRequestViewModel model)
    {
        try
        {
            HelpdeskRequest helpDeskRequest = new HelpdeskRequest
            {
                InsertedBy = model.EmployeeId,
                GroupId = model.GroupId,
                CategoryId = model.CategoryId,
                Priority = (int?)model.Priority,
                ServiceDetails = model.ServiceDetails,
                InsertedAt = DateTime.Now,
                StatusId = model.StatusId > 0 ? model.StatusId : 6,
                PendingAt = 3
            };
            
            ResponseViewModel response = await _helpDeskRepository.AddRequest(helpDeskRequest, model.selectedSubCategories);
            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = true,
                message = "Request Failed to Add: " + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> AddNotification(string notification)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 4,

        };        
        ResponseViewModel response = await _helpDeskRepository.AddNotification(addNotification);
        return response;
    }

    public async Task<AddHelpDeskRequestViewModel> GetEditDetails(long requestId)
    {
        try
        {
            HelpdeskRequest details = await _helpDeskRepository.GetDetails(requestId);
            if (details == null)
            {
                return new AddHelpDeskRequestViewModel();
            }

            var model = new AddHelpDeskRequestViewModel
            {
                HelpDeskRequestId = details.HelpdeskRequestId,
                EmployeeId = (int)details.InsertedBy,
                GroupId = details.Group.GroupId,
                CategoryId = details.Category.CategoryId,
                Priority = (HelpDeskEnum.Priority)details.Priority,
                ServiceDetails = details.ServiceDetails,
                RequestedDate = (DateTime)details.InsertedAt,
                StatusId = details.PendingAt == 3 || details.StatusId == 3 ? (int)details.StatusId  : Convert.ToInt32(details.StatusHistories.Where(s  => s.RequestId == details.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.Status).FirstOrDefault()),
                Group = details.Group.GroupName,
                Category = details.Category.CategoryName,
                SubCategories = details.SubcategoryMappings.Where(s => s.RequestId == requestId).Select(s => s.SubCategory.SubCategoryName).ToList(),
                InsertedBy = details.InsertedByNavigation.Name,
                Status = details.PendingAt == 3 || details.StatusId == 3|| details.PendingAt == 4? details.Status.StatusName : details.StatusHistories.Where(s  => s.RequestId == details.HelpdeskRequestId).OrderByDescending(s => s.UpdatedAt).Select(s => s.StatusNavigation.StatusName).FirstOrDefault(),
                selectedSubCategories = details.SubcategoryMappings
                        .Where(l => l.RequestId == requestId)
                        .Select(l => (int)l.SubCategoryId)
                        .ToArray()

            };
            return model;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<ResponseViewModel> EditRequest(AddHelpDeskRequestViewModel model)
    {
        HelpdeskRequest helpDeskRequest = await _helpDeskRepository.GetDetails(model.HelpDeskRequestId);
        {            
            helpDeskRequest.GroupId = model.GroupId;
            helpDeskRequest.CategoryId = model.CategoryId;
            helpDeskRequest.SubCategoryId = null;
            helpDeskRequest.Priority = (int?)model.Priority;
            helpDeskRequest.ServiceDetails = model.ServiceDetails;
            helpDeskRequest.InsertedAt = model.RequestedDate;
            helpDeskRequest.StatusId = model.StatusId > 0 ? model.StatusId : 6;
        };        
        ResponseViewModel response = await _helpDeskRepository.EditRequest(helpDeskRequest, model.selectedSubCategories);
        return response;
    }

    public async Task<ResponseViewModel> DeleteHelpDeskRequest(long requestId)
    {
        try
        {
            HelpdeskRequest request = await _helpDeskRepository.GetDetails(requestId);
            {
                request.DeletedAt = DateTime.Now;
            };

            ResponseViewModel response = await _helpDeskRepository.EditRequest(request,Array.Empty<int>());
            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Request Failed to Canceled:" + ex.Message
            };
        }
    }
    #endregion

    #region HelpDesk Response
    public async Task<HelpDeskPaginationViewModel> GetResponseData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string helpDeskResponseGroup, string helpDeskResponseStatus, int employeeId,string role)
    {
         try
        {
            return await _helpDeskRepository.GetPaginatedResponse(pageSize, pageNumber, search, sort, sortDirection, helpDeskResponseGroup, helpDeskResponseStatus, employeeId,role);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<ResponseViewModel> ResponseHelpDeskRequest(AddHelpDeskRequestViewModel model)
    {
        try
        {
            StatusHistory status = new StatusHistory
            {
                RequestId = model.HelpDeskRequestId,
                StatusChnageDate = DateTime.Now,
                Comment = model.Comment,
                UpdatedBy = model.EmployeeId,
                UpdatedAt = DateTime.Now,
                Status = model.StatusId
            };
            
            HelpdeskRequest request = await _helpDeskRepository.GetDetails(model.HelpDeskRequestId);
            {
                request.StatusId = model.StatusId == 2 || model.StatusId == 1  ? 6 : model.StatusId;    
                request.PendingAt = model.StatusId == 2 ? 4 : null;
            }

            ResponseViewModel response1 = await _helpDeskRepository.EditRequest(request);

            ResponseViewModel response = await _helpDeskRepository.AddStatus(status);
            if(response.success && response1.success)
            {
                return new ResponseViewModel{
                    success = true,
                    message = "Status Changed Successfully" 
                };
            }
            else if (!response.success)
            {   
                return new ResponseViewModel{
                    success = false,
                    message = "Error occur status change " + response.message 
                };
            }
            else{
                return new ResponseViewModel{
                    success = false,
                    message = "Error occur status change " + response1.message 
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

    public async Task<ResponseViewModel> AddNotificationByHr(string notification)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 4,
        };
        
        ResponseViewModel response = await _helpDeskRepository.AddNotificationByHr(addNotification);
        return response;
    }

    public async Task<ResponseViewModel> AddResponseNotification(string notification,int employeeId)
    {
        Notification addNotification = new Notification
        {
            Notification1 = notification,
            NotificationCategoryId = 4,
        };
        
        ResponseViewModel response = await _helpDeskRepository.AddResponseNotification(addNotification,employeeId);
        return response;
    }

    public async Task<List<StatusHistoryViewModel>> GetStatusHistory(long requestId)
    {
        return await _helpDeskRepository.GetStatusHistory(requestId);
    }
    #endregion

    #region HelpDesk Excel
    public async Task<byte[]> GetHelpDeskDataToExport(int pageSize, int pageNumber, string searchQuery, string helpDeskGroup, string helpDeskStatus, int employeeId)
    {
        return await _helpDeskRepository.GetHelpDeskDataToExport(pageSize, pageNumber, searchQuery,helpDeskGroup,helpDeskStatus,employeeId);
    }

    public async Task<byte[]> GetHelpDeskResponseDataToExport(int pageSize, int pageNumber, string searchQuery, string helpDeskGroup, string helpDeskStatus, int employeeId)
    {
        return await _helpDeskRepository.GetHelpDeskResponseDataToExport(pageSize, pageNumber, searchQuery,helpDeskGroup,helpDeskStatus,employeeId);
    }
    #endregion
}
