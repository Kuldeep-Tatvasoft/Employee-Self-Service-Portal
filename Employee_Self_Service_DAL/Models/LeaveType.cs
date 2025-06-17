using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class LeaveType
{
    public int TypeId { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequestEndLeaveTypeNavigations { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<LeaveRequest> LeaveRequestStartLeaveTypeNavigations { get; set; } = new List<LeaveRequest>();
}
