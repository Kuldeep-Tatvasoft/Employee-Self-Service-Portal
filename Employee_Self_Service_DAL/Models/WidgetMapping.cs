using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class WidgetMapping
{
    public long WidgetMappingId { get; set; }

    public long? WidgetId { get; set; }

    public int? EmployeeId { get; set; }

    public bool? IsVisible { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Widget? Widget { get; set; }
}
