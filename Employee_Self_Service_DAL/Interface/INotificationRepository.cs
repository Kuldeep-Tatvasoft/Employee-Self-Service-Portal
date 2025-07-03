using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_DAL.Interface;

public interface INotificationRepository
{
    Task<List<NotificationViewModel>> GetNotifications(int employeeId);
    Task<ResponseViewModel> MarkRead(int employeeId,long notificationId);
    Task<int> GetUnreadNotificationCount(int employeeId);
}
