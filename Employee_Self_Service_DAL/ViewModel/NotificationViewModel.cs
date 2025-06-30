using System;

namespace Employee_Self_Service_DAL.ViewModel;

public class NotificationViewModel
{   
    public long NotificationId { get; set; }
    public string Message { get; set; }
    public bool? IsRead { get; set; }
    public string NotificationCategory { get; set;}
    public int CategoryId {get; set;}
}
