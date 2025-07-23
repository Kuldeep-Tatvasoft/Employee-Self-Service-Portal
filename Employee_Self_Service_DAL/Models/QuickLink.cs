using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class QuickLink
{
    public long QuickLinkId { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }
}
