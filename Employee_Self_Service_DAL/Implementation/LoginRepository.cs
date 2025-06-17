// using System.Runtime.CompilerServices;
// using Employee_Self_Service_DAL.Data;
// using Employee_Self_Service_DAL.Interface;
// using Employee_Self_Service_DAL.Models;
// using Employee_Self_Service_DAL.ViewModel;
// using Microsoft.EntityFrameworkCore;

// namespace Employee_Self_Service_DAL.Implementation;

// public class LoginRepository : ILoginRepository
// {
//     private readonly EmployeeSelfServiceContext _context;

//     public LoginRepository(EmployeeSelfServiceContext context)
//     {
//         _context = context;
//     }

    

//     public  Employee GetUserByEmail(string email)
//     {
//         return  _context.Employees.Include(u => u.Role).FirstOrDefault(u => u.Email == email );
//     }

//     public async Task<ResponseViewModel> UpdateEmployee(Employee employee)
//     {   
//         try{
//             _context.Employees.Update(employee);
//             await _context.SaveChangesAsync();
//             return new ResponseViewModel{
//                 success = true,
//                 message = "Password Reset Successfully"
//             };
//         }
//         catch (Exception ex)
//         {
//             return new ResponseViewModel{
//                 success = false,
//                 message = "Error Occur Reset Password" + ex.Message
//             };
//         }
       
    
//     }
//     public async Task<ResponseViewModel> RegisterUser(Employee employee)
//     {
//         try
//         {   
//             _context.Add(employee);
//             await _context.SaveChangesAsync();
//             return new ResponseViewModel{
//                 success = true,
//                 message = "Registration Successfully"
//             };
//         }
//         catch(Exception ex)
//         {
//             return new ResponseViewModel{
//                 success = false,
//                 message = "Error occur Register User" + ex.Message
//             };

//         }
//     }
// }