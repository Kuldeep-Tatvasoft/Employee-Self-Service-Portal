using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;

namespace Employee_Self_Service_BAL.Implementation;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    
}
