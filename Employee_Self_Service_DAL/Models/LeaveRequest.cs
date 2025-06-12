using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class LeaveRequest
{
    public int Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int EmployeeId { get; set; }

    public int ApprovedBy { get; set; }

    public int? StatusId { get; set; }

    public bool? AdhocLeave { get; set; }

    public bool? AvailableOnPhone { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? ReasonDescription { get; set; }

    public int? LeaveType { get; set; }

    public int? Reason { get; set; }

    public DateTime? ReturnDate { get; set; }

    public virtual Employee ApprovedByNavigation { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual LeaveType? LeaveTypeNavigation { get; set; }

    public virtual Reason? ReasonNavigation { get; set; }

    public virtual RequestStatus? Status { get; set; }
}
