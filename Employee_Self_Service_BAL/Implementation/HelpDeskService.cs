using System.Text.RegularExpressions;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Constants;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Group = Employee_Self_Service_DAL.Models.Group;

namespace Employee_Self_Service_BAL.Implementation;

public class HelpDeskService : IHelpDeskService
{
    private readonly IHelpDeskRepository _helpDeskRepository;

    public HelpDeskService(IHelpDeskRepository helpDeskRepository)
    {
        _helpDeskRepository = helpDeskRepository;
    }

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
    public async Task<HelpDeskPaginationViewModel> GetRequestData(int pageSize, int pageNumber, string search, string sort, string sortDirection, string helpDeskRequestGroup, string helpDeskRequestStatus,int employeeId)
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
            HelpdeskRequest request = new HelpdeskRequest
            {
                GroupId = model.GroupId,
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCategoryId,
                Priority = (int?)model.Priority,
                ServiceDetails = model.ServiceDetails,
                StatusId = 6,
                InsertedAt = model.RequestedDate,
                InsertedBy = model.EmployeeId
            };
            ResponseViewModel response  = await _helpDeskRepository.AddRequest(request);
            return response;
        }
        catch(Exception ex)
        {
            return new ResponseViewModel{
                success = true,
                message = "Request Failed to Add: " + ex.Message
            };
        }
    }

    public async Task<AddHelpDeskRequestViewModel> GetEditDetails(long requestId)
    {
        HelpdeskRequest details = await _helpDeskRepository.GetDetails(requestId);
        if(details == null)
        {
            return null;
        }
        AddHelpDeskRequestViewModel model = new AddHelpDeskRequestViewModel
        {
            HelpDeskRequestId = details.HelpdeskRequestId,
            EmployeeId = (int)details.InsertedBy,
            RequestedDate = (DateTime)details.InsertedAt,
            GroupId = (int)details.GroupId,
            CategoryId = (int)details.CategoryId,
            SubCategoryId = (int)details.SubCategoryId,
            Group = details.Group.GroupName,
            Category = details.Category.CategoryName,
            SubCategory = details.SubCategory.SubCategoryName,
            Priority = (HelpDeskEnum.Priority)details.Priority,
            ServiceDetails = details.ServiceDetails,
            Status = details.Status.StatusName,
            // StatusId = (int)h.StatusId
        };
        return model;
    }

    public async Task<ResponseViewModel> EditRequest(AddHelpDeskRequestViewModel model)
    {
        try
        {
            HelpdeskRequest request = await _helpDeskRepository.GetDetails(model.HelpDeskRequestId);
            {
                request.GroupId = model.GroupId;
                request.CategoryId = model.CategoryId;
                request.SubCategoryId = model.SubCategoryId;
                request.Priority = (int?)model.Priority;
                request.ServiceDetails = model.ServiceDetails;
            }
            ResponseViewModel response = await _helpDeskRepository.EditRequest(request);
            return response;
        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = true,
                message = "HelpDesk Request Failed to Edit" + ex.Message
            };
        }
    }
}
