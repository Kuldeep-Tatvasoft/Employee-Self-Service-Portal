using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;

namespace Employee_Self_Service_DAL.Implementation;

public class ProfileRepository : IProfileRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public ProfileRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }
}
