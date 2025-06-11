using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class ProfileRepository : IProfileRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public ProfileRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<Employee> GetUserDetails(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UpdateProfile(ProfileViewModel model)
    {
        try
        {   
            var user = await _context.Employees.FirstOrDefaultAsync(u => u.EmployeeId == model.EmployeeId);
            if (user == null)
            {
                return false;
            }
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Phone = long.Parse(model.ContactNo);
            user.BloodGroup = model.BloodGroup;
            user.AnyDiseases = model.AnyDiseases;
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

                user.ProfileImage = $"/uploads/{fileName}"; // Store relative path in DB
            }
            _context.Employees.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<ResponseViewModel> ChangePassword(Employee user)
    {
        _context.Employees.Update(user);
        await _context.SaveChangesAsync();

        return new ResponseViewModel
        {
            success = true,
            message = "Password changes successfully"
        };
    }
}
