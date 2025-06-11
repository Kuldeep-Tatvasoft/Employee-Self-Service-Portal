using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly ILoginService _loginService;
    public ProfileService(IProfileRepository profileRepository, ILoginService loginService)
    {
        _profileRepository = profileRepository;
        _loginService = loginService;
    }

    public async Task<ProfileViewModel> GetUserDetails(string email)
    {
       try
       {
        Employee details =  await _profileRepository.GetUserDetails(email);
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

    public async Task<bool> UpdateProfile(ProfileViewModel model)
    {
        return await _profileRepository.UpdateProfile(model);
    }

    public async Task<ResponseViewModel> ChangePassword(LoginViewModel model)
    {
        try
        {   
            Employee user = _loginService.GetUserByEmail(model.Email);
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
            ResponseViewModel response = await _profileRepository.ChangePassword(user);
            return response;
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                message = ex.Message,
                success = false
            };
        }
    } 

}
