using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface INotificationRepository
{
    Task<List<NotificationViewModel>> GetNotifications(long roleId);
    Task<ResponseViewModel> MarkRead(long notificationId);
    Task<int> GetUnreadNotificationCount(long roleId);
}
