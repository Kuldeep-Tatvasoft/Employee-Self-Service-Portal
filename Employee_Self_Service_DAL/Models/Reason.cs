using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Reason
{
    public int ReasonId { get; set; }

    public string? Reason1 { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
