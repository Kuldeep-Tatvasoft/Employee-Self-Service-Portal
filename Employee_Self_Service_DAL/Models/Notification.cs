using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Notification
{
    public long NotificationId { get; set; }

    public string? Notification1 { get; set; }

    public int? NotificationCategoryId { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CatrgoryId { get; set; }

    public virtual NotificationCategory? NotificationCategory { get; set; }
}
