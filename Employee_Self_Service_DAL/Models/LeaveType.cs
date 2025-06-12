using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class LeaveType
{
    public int TypeId { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
