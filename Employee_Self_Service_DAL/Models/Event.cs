using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Event
{
    public long EventId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }
}
