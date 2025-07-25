using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Widget
{
    public long WidgetId { get; set; }

    public string? CardName { get; set; }

    public bool? IsVisible { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<WidgetMapping> WidgetMappings { get; set; } = new List<WidgetMapping>();
}
