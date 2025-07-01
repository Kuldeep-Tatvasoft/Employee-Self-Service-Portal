using Employee_Self_Service_DAL.ViewModel;

public class NotificationGroupViewModel
{
    public int NotificationCategoryId { get; set; }
    public string NotificationCategoryName { get; set; }
    public List<NotificationViewModel> Notifications { get; set; }
}