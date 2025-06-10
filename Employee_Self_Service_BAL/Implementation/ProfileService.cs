using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
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
            ContactNo = details.Phone.ToString(),
            DateOfBirth = (DateOnly)details.DateOfBirth
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

}
