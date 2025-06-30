using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class NotificationCategory
{
    public int NotificationCategoryId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
