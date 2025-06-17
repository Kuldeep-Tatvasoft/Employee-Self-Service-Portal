using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Employee_Self_Service_BAL.Implementation;

public class ProfileService : IProfileService
{
    // private readonly IProfileRepository _profileRepository;
    private readonly IEmployeeRepository _employeeRepository;
    // private readonly ILoginRepository _loginRepository;
    public ProfileService(IEmployeeRepository employeeRepository)
    {
        // _profileRepository = profileRepository;
        // _loginRepository = loginRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<ProfileViewModel> GetUserDetails(string email)
    {
       try
       {
        Employee details = _employeeRepository.GetUserByEmail(email);
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
            ContactNo = details.Phone.ToString() ?? null,
            DateOfBirth = (DateOnly)details.DateOfBirth,
            BloodGroup = details.BloodGroup,
            AnyDiseases = details.AnyDiseases ?? null
        };
        return model;
       }
       catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;    
        }
    }

    public async Task<ResponseViewModel> UpdateProfile(ProfileViewModel model)
    {
        try
        {   
            Employee employee = _employeeRepository.GetUserByEmail(model.Email);
            {
                employee.EmployeeId = model.EmployeeId;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Gender = model.Gender;
                employee.Phone = long.Parse(model.ContactNo);
                employee.BloodGroup = model.BloodGroup;
                employee.AnyDiseases = model.AnyDiseases;
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
            }
            ResponseViewModel response = await _employeeRepository.UpdateProfile(employee);
            return response;            
        }
        catch(Exception ex)
        {
            return new ResponseViewModel{
                success = false,
                message = "Failed to update profile:" + ex.Message
            };
        }
    }

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
            ResponseViewModel response = await _employeeRepository.ChangePassword(user);
            return response;
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

}
