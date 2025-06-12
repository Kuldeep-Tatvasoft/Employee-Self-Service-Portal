using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class RequestStatus
{
    public int StatusId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
