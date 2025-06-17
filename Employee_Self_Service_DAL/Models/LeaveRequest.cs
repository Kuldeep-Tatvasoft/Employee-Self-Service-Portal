using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class LeaveRequest
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int EmployeeId { get; set; }

    public int? ApprovedBy { get; set; }

    public int? StatusId { get; set; }

    public bool? AdhocLeave { get; set; }

    public bool? AvailableOnPhone { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? ReasonDescription { get; set; }

    public int? StartLeaveType { get; set; }

    public int? Reason { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public decimal? ActualLeaveDuration { get; set; }

    public decimal? TotalLeaveDuration { get; set; }

    public string? AlternatePhoneMo { get; set; }

    public int LeaveRequestId { get; set; }

    public int? EndLeaveType { get; set; }

    public virtual Employee? ApprovedByNavigation { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual LeaveType? EndLeaveTypeNavigation { get; set; }

    public virtual Reason? ReasonNavigation { get; set; }

    public virtual LeaveType? StartLeaveTypeNavigation { get; set; }

    public virtual RequestStatus? Status { get; set; }
}
