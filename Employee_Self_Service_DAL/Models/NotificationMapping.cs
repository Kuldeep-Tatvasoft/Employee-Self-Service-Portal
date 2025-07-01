using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class NotificationMapping
{
    public int NotificationMappingId { get; set; }

    public long? NotificationId { get; set; }

    public int? RoleId { get; set; }

    public int? UserId { get; set; }

    public virtual Notification? Notification { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Employee? User { get; set; }
}
