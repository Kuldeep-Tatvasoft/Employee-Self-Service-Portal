using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface INotificationService
{
    Task<List<NotificationViewModel>> GetNotifications(int employeeId);
    Task<ResponseViewModel> MarkRead(int employeeId,long notificationId);
    Task<int> GetNotificationCount(int employeeId);
}
