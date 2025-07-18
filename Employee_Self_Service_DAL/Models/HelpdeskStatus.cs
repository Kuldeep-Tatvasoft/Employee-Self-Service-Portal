using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class HelpdeskStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<HelpdeskRequest> HelpdeskRequests { get; set; } = new List<HelpdeskRequest>();

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
