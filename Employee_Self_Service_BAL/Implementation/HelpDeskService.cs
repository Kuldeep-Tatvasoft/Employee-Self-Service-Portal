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
            var helpDeskRequest = new HelpdeskRequest
        {
            InsertedBy = model.EmployeeId,
            GroupId = model.GroupId,
            CategoryId = model.CategoryId,
            // SubCategoryId = model.CategoryId == 3 ? null : model.SubCategoryId,
            Priority = (int?)model.Priority,
            ServiceDetails = model.ServiceDetails,
            InsertedAt = model.RequestedDate,
            StatusId = model.StatusId > 0 ? model.StatusId : 6 
        };
            // HelpdeskRequest request = new HelpdeskRequest
            // {
            //     GroupId = model.GroupId,
            //     CategoryId = model.CategoryId,
            //     SubCategoryId = model.SubCategoryId,
            //     Priority = (int?)model.Priority,
            //     ServiceDetails = model.ServiceDetails,
            //     StatusId = 6,
            //     InsertedAt = model.RequestedDate,
            //     InsertedBy = model.EmployeeId
            // };
            ResponseViewModel response  = await _helpDeskRepository.AddRequest(helpDeskRequest,model.selectedSubCategories);
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

    // public async Task<AddHelpDeskRequestViewModel> GetEditDetails(long requestId)
    // {
    //     HelpdeskRequest details = await _helpDeskRepository.GetDetails(requestId);
    //     if(details == null)
    //     {
    //         return null;
    //     }
    //     AddHelpDeskRequestViewModel model = new AddHelpDeskRequestViewModel
    //     {
    //         HelpDeskRequestId = details.HelpdeskRequestId,
    //         EmployeeId = (int)details.InsertedBy,
    //         RequestedDate = (DateTime)details.InsertedAt,
    //         GroupId = (int)details.GroupId,
    //         CategoryId = (int)details.CategoryId,
    //         SubCategoryId = (int)details.SubCategoryId,
    //         Group = details.Group.GroupName,
    //         Category = details.Category.CategoryName,
    //         SubCategory = details.SubCategory.SubCategoryName,
    //         Priority = (HelpDeskEnum.Priority)details.Priority,
    //         ServiceDetails = details.ServiceDetails,
    //         Status = details.Status.StatusName,
    //         // StatusId = (int)h.StatusId
    //     };
    //     return model;
    // }

    public async Task<AddHelpDeskRequestViewModel> GetEditDetails(long requestId)
    {   
        try{
            HelpdeskRequest details = await _helpDeskRepository.GetDetails(requestId);
        if(details == null)
        {
            return new AddHelpDeskRequestViewModel();
        }

        var model = new AddHelpDeskRequestViewModel
        {
            HelpDeskRequestId = details.HelpdeskRequestId,
            EmployeeId = (int)details.InsertedBy,
            GroupId = details.Group.GroupId,
            CategoryId = details.Category.CategoryId,
            // SubCategoryId = details.SubCategory?.SubCategoryId ,
            Priority = (HelpDeskEnum.Priority)details.Priority,
            ServiceDetails = details.ServiceDetails,
            RequestedDate = (DateTime)details.InsertedAt,
            StatusId = (int)details.StatusId,
            Group = details.Group.GroupName,
            Category = details.Category.CategoryName,
            // SubCategory = details.SubCategory.SubCategoryName,
            Status = details.Status.StatusName,
            selectedSubCategories = details.SubcategoryMappings
                    .Where(l => l.RequestId == requestId)
                    .Select(l => (long)l.SubCategoryId)
                    .ToArray()
                
        };
        return model;
        }
        catch(Exception ex)
        {   
            Console.WriteLine(ex.Message);
            return null;
        }
        

        
    }

    public async Task<ResponseViewModel> EditRequest(AddHelpDeskRequestViewModel model)
    {
        HelpdeskRequest helpDeskRequest = await _helpDeskRepository.GetDetails(model.HelpDeskRequestId);
        // if(helpDeskRequest.CategoryId == 3)
        // {
        //     var existingLinks = helpDeskRequest.SubcategoryMappings
        //         .Where(l => l.RequestId == model.HelpDeskRequestId);
        //     helpDeskRequest.SubcategoryMappings.RemoveRange(existingLinks);
        // }
        helpDeskRequest.HelpdeskRequestId = model.HelpDeskRequestId;
        helpDeskRequest.GroupId = model.GroupId;
        helpDeskRequest.CategoryId = model.CategoryId;
        helpDeskRequest.SubCategoryId = model.CategoryId == 3 ? null : model.SubCategoryId;
        helpDeskRequest.Priority = (int?)model.Priority;
        helpDeskRequest.ServiceDetails = model.ServiceDetails;
        helpDeskRequest.InsertedAt = model.RequestedDate;
        helpDeskRequest.StatusId = model.StatusId;

        

        ResponseViewModel response = await _helpDeskRepository.EditRequest(helpDeskRequest,model.selectedSubCategories,model.CategoryId);
        return response;
        // _context.Update(helpDeskRequest);

        // if (model.CategoryId == 3)
        // {
        //     var existingLinks = _context.HelpdeskSubCategoryLinks
        //         .Where(l => l.HelpDeskRequestId == model.HelpDeskRequestId);
        //     _context.HelpdeskSubCategoryLinks.RemoveRange(existingLinks);

        //     foreach (var subCategoryId in model.selectedSubCategories)
        //     {
        //         _context.HelpdeskSubCategoryLinks.Add(new HelpdeskSubCategoryLink
        //         {
        //             HelpDeskRequestId = model.HelpDeskRequestId,
        //             SubCategoryId = (int)subCategoryId
        //         });
        //     }
        // }

        // await _context.SaveChangesAsync();
        // return new ResponseViewModel { Success = true, Message = "Request updated successfully." };
    }
    // public async Task<ResponseViewModel> EditRequest(AddHelpDeskRequestViewModel model)
    // {
    //     try
    //     {
    //         HelpdeskRequest request = await _helpDeskRepository.GetDetails(model.HelpDeskRequestId);
    //         {
    //             request.GroupId = model.GroupId;
    //             request.CategoryId = model.CategoryId;
    //             request.SubCategoryId = model.SubCategoryId;
    //             request.Priority = (int?)model.Priority;
    //             request.ServiceDetails = model.ServiceDetails;
    //         }
    //         ResponseViewModel response = await _helpDeskRepository.EditRequest(request);
    //         return response;
    //     }
    //     catch(Exception ex)
    //     {
    //         return new ResponseViewModel
    //         {
    //             success = true,
    //             message = "HelpDesk Request Failed to Edit" + ex.Message
    //         };
    //     }
    // }
}
