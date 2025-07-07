using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<GroupCategory> GroupCategories { get; set; } = new List<GroupCategory>();

    public virtual ICollection<HelpdeskRequest> HelpdeskRequests { get; set; } = new List<HelpdeskRequest>();
}
