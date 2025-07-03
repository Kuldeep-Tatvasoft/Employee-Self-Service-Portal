using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Implementation;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
       
    }

    public async Task<List<NotificationViewModel>> GetNotifications(int employeeId)
    {
        return await _notificationRepository.GetNotifications(employeeId);
    }

    public async Task<ResponseViewModel> MarkRead(int employeeId,long notificationId)
    {
        return await _notificationRepository.MarkRead(employeeId,notificationId);
    }

    public async Task<int> GetNotificationCount(int employeeId)
    {
        
        return await _notificationRepository.GetUnreadNotificationCount(employeeId);
    }
}
