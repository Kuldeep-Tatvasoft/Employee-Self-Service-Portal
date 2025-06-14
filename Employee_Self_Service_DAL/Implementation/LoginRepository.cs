using System.Runtime.CompilerServices;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class LoginRepository : ILoginRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public LoginRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    // public LoginViewModel GetUserById(long id)
    // {
    //     Employee? employee = _context.Employees.Include(x => x.Role).FirstOrDefault(u => u.EmployeeId == id);
    //     LoginViewModel login = new LoginViewModel{
    //         EmployeeId = employee.EmployeeId,
    //         Role = employee.Role.Role1,
    //         ProfileImage = employee.ProfileImage,
    //         EmployeeName = employee.Name
    //     };

    //     return login;
    // }

    public  Employee GetUserByEmail(string email)
    {
        return  _context.Employees.Include(u => u.Role).FirstOrDefault(u => u.Email == email );
    }

    public async Task<bool> UpdatePassword(Employee employee)
    {   
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> ExistUserByEmail(string Email)
    {
        try
        {
            var email =  _context.Employees.FirstOrDefault(x => x.Email == Email);
            if (email == null)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            
            return false; 
        }
    }
    public async Task<bool> RegisterUser(Employee employee)
    {
        try
        {   
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}