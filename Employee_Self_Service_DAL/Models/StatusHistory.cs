using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class StatusHistory
{
    public long StatusHistoryId { get; set; }

    public long RequestId { get; set; }

    public DateTime? StatusChnageDate { get; set; }

    public string? Comment { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Status { get; set; }

    public virtual HelpdeskRequest Request { get; set; } = null!;

    public virtual HelpdeskStatus? StatusNavigation { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
