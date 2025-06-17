// using Employee_Self_Service_DAL.Data;
// using Employee_Self_Service_DAL.Interface;
// using Employee_Self_Service_DAL.Models;
// using Employee_Self_Service_DAL.ViewModel;
// using Microsoft.EntityFrameworkCore;

// namespace Employee_Self_Service_DAL.Implementation;

// public class ProfileRepository : IProfileRepository
// {
//     private readonly EmployeeSelfServiceContext _context;

//     public ProfileRepository(EmployeeSelfServiceContext context)
//     {
//         _context = context;
//     }
//     public async Task<ResponseViewModel> UpdateProfile(Employee employee)
//     {
//         try
//         {   
//             var user = await _context.Employees.FirstOrDefaultAsync(u => u.EmployeeId == employee.EmployeeId);
//             if (employee == null)
//             {
//                 return new ResponseViewModel
//                 {
//                     success = false,
//                     message = "Employee not found"
//                 };
//             }
            
//             _context.Employees.Update(user);
//             await _context.SaveChangesAsync();
//             return new ResponseViewModel
//             {
//                 success = true,
//                 message = "Profile updated successfully."
//             };
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine(ex.Message);
//             return new ResponseViewModel{
//                 success = false,
//                 message = "Error updating profile:" + ex.Message
//             };
//         }
//     }

//     public async Task<ResponseViewModel> ChangePassword(Employee user)
//     {   
//         try{
//         _context.Employees.Update(user);
//         await _context.SaveChangesAsync();

//         return new ResponseViewModel
//         {
//             success = true,
//             message = "Password changes successfully"
//         };
//         }
//         catch (Exception ex)
//         {
//             return new ResponseViewModel
//             {
//                 success = false,
//                 message = "Error changing password:" + ex.Message
//             };
//         }
//     }
// }
