using System.Runtime.CompilerServices;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service_BAL.Implementation;

public class ProfileService : IProfileService
{
    private readonly IEmployeeRepository _employeeRepository;
    public ProfileService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    #region  My Profile
    public async Task<ProfileViewModel> GetUserDetails(string email)
    {
        try
        {
            Employee details = _employeeRepository.GetUserByEmail(email);
            if(details == null)
            {
                return null;
            }
            ProfileViewModel model = new ProfileViewModel
            {
                EmployeeId = details.EmployeeId,
                ProfileImage = details.ProfileImage,
                Name = details.Name,
                Email = details.Email,
                StartingDate = DateOnly.FromDateTime((DateTime)details.CreatedAt),
                Experience = DateTime.Now.Year - DateTime.Parse(details.CreatedAt.ToString()).Year,
                Department = details.Department,
                Designation = details.Designation,
                ContactNo = details.Phone.ToString(),
                DateOfBirth = details.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                Gender = details.Gender,
                BloodGroup = details.BloodGroup ?? string.Empty,
                AnyDiseases = details.AnyDiseases ?? null
            };
            return model;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<ResponseViewModel> UpdateProfile(ProfileViewModel model, HttpContext httpContext)
    {
        try
        {
            Employee employee = _employeeRepository.GetUserByEmail(model.Email);
            {
                
                if(model.EmployeeId !=0){
                employee.DateOfBirth = string.IsNullOrWhiteSpace(model.DateOfBirth)
                    ? null
                    : DateOnly.Parse(model.DateOfBirth);
                employee.Gender = model.Gender;
                employee.Phone = long.Parse(model.ContactNo);
                employee.BloodGroup = model.BloodGroup;
                employee.AnyDiseases = model.AnyDiseases;
                }
            };

            if (model.FormFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.FormFile.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(fileStream);
                }

                employee.ProfileImage = $"/uploads/{fileName}";
                httpContext.Response.Cookies.Append("profileImage", employee.ProfileImage ?? "/images/Default_pfp.svg.png");
                
            }
            ResponseViewModel response = await _employeeRepository.UpdateEmployee(employee);
            
            if (response.success)
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Profile updated successfully."
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
                message = "Failed to update profile:" + ex.Message
            };
        }
    }
    #endregion

    #region Change Password
    public async Task<ResponseViewModel> ChangePassword(LoginViewModel model)
    {
        try
        {
            Employee user = _employeeRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                return new ResponseViewModel
                {
                    success = false,
                    message = "User not found"
                };
            }
            bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!isPasswordMatch)
            {
                return new ResponseViewModel
                {
                    success = false,
                    message = "Current password is incorrect"
                };
            }
            user.Password = model.NewPassword;
            String hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashPassword;
            ResponseViewModel response = await _employeeRepository.UpdateEmployee(user);
            if (response.success)
            {
                return new ResponseViewModel
                {
                    success = response.success,
                    message = "Password changes successfully"
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
                message = "Failed to changed Password:" + ex.Message,
                success = false
            };
        }
    }
    #endregion

    public async Task<ProfileViewModel> GetEmployeeById(int employeeId)
    {
        try
        {
            Employee details = _employeeRepository.GetEmployeeById(employeeId);
            if(details == null)
            {
                return null;
            }
            ProfileViewModel model = new ProfileViewModel
            {
                EmployeeId = details.EmployeeId,
                ProfileImage = details.ProfileImage,
                Name = details.Name,
                Email = details.Email,
                // ReportingPerson = details.ReportiingPerson,
                StartingDate = DateOnly.FromDateTime((DateTime)details.CreatedAt),
                Experience = DateTime.Now.Year - DateTime.Parse(details.CreatedAt.ToString()).Year,
                Department = details.Department,
                Designation = details.Designation,
                ContactNo = details.Phone.ToString(),
                DateOfBirth = details.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                Gender = details.Gender,
                BloodGroup = details.BloodGroup ?? string.Empty,
                AnyDiseases = details.AnyDiseases ?? null
            };
            return model;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<WidgetMapping>> GetWidgets(int employeeId)
    {
        return await _employeeRepository.GetWidgets(employeeId);
    }

    public async Task<ResponseViewModel> AddRemoveWidget(long widgetId,int employeeId)
    {
        try
        {
            List<WidgetMapping> widgets = await _employeeRepository.GetWidgets(employeeId);
            WidgetMapping widget = widgets.FirstOrDefault(w => w.WidgetId == widgetId);
            if((bool)widget.IsVisible)
            {
                widget.IsVisible = false;                
            }
            else
            {
                widget.IsVisible = true;                
            }
            ResponseViewModel response = await _employeeRepository.UpdateWidget(widget);
            if(response.success)
            {   
                if((bool)widget.IsVisible){
                    return new ResponseViewModel
                    {   
                        success = true,
                        message = "Widget Added successfully"
                    };  
                }
                else{
                    return new ResponseViewModel
                    {
                        success = true,
                        message = "Widget remove successfully"
                    };  
                }
            }
            else{
                return new ResponseViewModel
                {   
                    success = false,
                    message = "Error occur update widget:" + response.message
                };
            }
        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur while adding/removing widget:" +ex.Message 
            };
        }
    }

    // public async Task<List<QuickLink>> GetQuickLink()
    // {
    //     return await _employeeRepository.GetQuickLink();     
    // }
    public async Task<List<QuickLinkViewModel>> GetQuickLink(int employeeId)
    {
        return await _employeeRepository.GetQuickLink(employeeId);
    }

    public async Task<ResponseViewModel> AddQuickLink(List<QuickLinkViewModel> links, int employeeId)
    {
        try
        {
            // List<QuickLink> quickLinks = await _employeeRepository.GetQuickLinks();
            // if (quickLinks == null)
            // {
            //     quickLinks = new List<QuickLink>();
            // }
            // foreach(var link in links)
            // {
                
            // }
            return await _employeeRepository.AddQuickLink(links,employeeId);

        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur while adding quick link:" + ex.Message
            };
        }
    }

    public async Task<ResponseViewModel> DeleteQuickLink(long quickLinkId)
    {
        try
        {
            List<QuickLink> quickLinkViews = await _employeeRepository.GetQuickLinks();
            QuickLink quickLink = quickLinkViews.FirstOrDefault(q => q.QuickLinkId == quickLinkId);
            {
                if (quickLink == null)
                {
                    return new ResponseViewModel
                    {
                        success = false,
                        message = "Quick Link not found"
                    };
                }

                quickLink.IsDeleted = true;
                ResponseViewModel response = await _employeeRepository.UpdateQuickLink(quickLink);
                if(response.success)
                {
                    return new ResponseViewModel
                    {
                        success = response.success,
                        message = "Quick Link deleted successfully."
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        success = response.success,
                        message = "Error occur deleting link:" + response.message
                    };
                }

            }
        }
        catch(Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Filed to Delete Quick Link:" + ex.Message
            };
        }
    }
}
