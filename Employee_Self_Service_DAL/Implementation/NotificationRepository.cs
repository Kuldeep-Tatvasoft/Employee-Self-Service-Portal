using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class NotificationRepository : INotificationRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public NotificationRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    

    public async Task<List<NotificationViewModel>> GetNotifications(int employeeId)
    {   
        
        return await _context.NotificationMappings
            .Include(n => n.Notification)
            .Include(n => n.Role)
            .Include(n => n.User)
            .Where(n => n.ReadMark == false && n.UserId == employeeId)
            .OrderByDescending(n => n.Notification.CreatedAt)
            .Select(n => new NotificationViewModel
            {   
                NotificationCategoryId = (int)n.Notification.NotificationCategoryId,
                NotificationId = n.Notification.NotificationId,
                Message = n.Notification.Notification1,
                NotificationCategory = n.Notification.NotificationCategory.Category,
                CategoryId = (int)n.Notification.CategoryId,
                
            }).ToListAsync();
    }

    public async Task<ResponseViewModel> MarkRead(int employeeId)
    {   try{
        var notification = await _context.NotificationMappings.Where(u => u.UserId == employeeId).FirstOrDefaultAsync();

        notification.ReadMark = true;
        _context.NotificationMappings.Update(notification);
        await _context.SaveChangesAsync();
        return new ResponseViewModel{
            success = true
        };
        }
        catch(Exception ex){
            return new ResponseViewModel{
                success = false
            };
        }
        
    }

    public async Task<int> GetUnreadNotificationCount(int employeeId)
    {   
        
        return await _context.NotificationMappings.Include(n => n.Notification).CountAsync(n => n.ReadMark == false && n.UserId == employeeId);
    }
}
