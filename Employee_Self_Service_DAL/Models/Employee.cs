using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Employee
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public long Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? RoleId { get; set; }

    public string Department { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? ProfileImage { get; set; }

    public int EmployeeId { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequestApprovedByNavigations { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<LeaveRequest> LeaveRequestEmployees { get; set; } = new List<LeaveRequest>();

    public virtual Role? Role { get; set; }
}
