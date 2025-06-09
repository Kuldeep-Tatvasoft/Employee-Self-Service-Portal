using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;

    public LoginService(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    #region Login
    public LoginViewModel GetUserById(long employeeId)
    {   
        return _loginRepository.GetUserById(employeeId);
        
    }

    public Employee GetUserByEmail(string email)
    {
        return _loginRepository.GetUserByEmail(email);
    }

    public LoginViewModel Login(LoginViewModel model)
    {
        try{
            var loginCredential = _loginRepository.GetUserByEmail(model.Email);
            if (loginCredential == null)
            {
                
                return new LoginViewModel{
                    Email = "Email is not valid"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, loginCredential.Password);
            if (!isPasswordValid)
            {
                return new LoginViewModel{
                    Password = "Password is not valid"
                };
            }

            if (loginCredential.IsActive == false)
            {
                return null; 
            }

            return GetUserById(loginCredential.EmployeeId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    #endregion
    
    
    public async Task<bool> UpdatePassword(Employee employee)
    {
       return await _loginRepository.UpdatePassword(employee);
    }
    
    public async Task<bool> ExistUserByEmail(string Email)
    {
        try
        {
            return await _loginRepository.ExistUserByEmail(Email);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }
    }
    public async Task<bool> RegisterUser (RegisterViewModel model)
    {
        try
        {   String hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); 
            Employee employee = new Employee
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashPassword,
                RoleId = model.RoleId,
                Department = model.Department,
                Designation = model.Designation,
                Phone = long.TryParse(model.Phone, out var phone) ? phone : 0
            };
            if(model.ProfileImage != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyToAsync(fileStream);
                }

                employee.ProfileImage = $"/uploads/{fileName}";
            }

            return await _loginRepository.RegisterUser(employee);
            
            
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    } 
}