using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class LeaveRequest
{
    public int Id { get; set; }

    public string LeaveType { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Reason { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int EmployeeId { get; set; }

    public int ApprovedBy { get; set; }

    public virtual Employee ApprovedByNavigation { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
