using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class EventCategory
{
    public int CategoryId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
