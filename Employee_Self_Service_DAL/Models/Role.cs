using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<HelpdeskRequest> HelpdeskRequests { get; set; } = new List<HelpdeskRequest>();
}
