using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface INotificationService
{
    Task<List<NotificationViewModel>> GetNotifications(long roleId);
    Task<ResponseViewModel> MarkRead(long notificationId);
    Task<int> GetNotificationCount(long roleId);
}
