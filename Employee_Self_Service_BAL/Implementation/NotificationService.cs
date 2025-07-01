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

    public async Task<List<NotificationViewModel>> GetNotifications(long roleId)
    {
        return await _notificationRepository.GetNotifications(roleId);
    }

    public async Task<ResponseViewModel> MarkRead(long notificationId)
    {
        return await _notificationRepository.MarkRead(notificationId);
    }

    public async Task<int> GetNotificationCount(long roleId)
    {
        
        return await _notificationRepository.GetUnreadNotificationCount(roleId);
    }
}
