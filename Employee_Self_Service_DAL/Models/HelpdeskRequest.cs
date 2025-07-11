using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class HelpdeskRequest
{
    public long HelpdeskRequestId { get; set; }

    public int? GroupId { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string? ServiceDetails { get; set; }

    public int? StatusId { get; set; }

    public DateTime? InsertedAt { get; set; }

    public int? InsertedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? Priority { get; set; }

    public virtual GroupCategory? Category { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Employee? InsertedByNavigation { get; set; }

    public virtual HelpdeskStatus? Status { get; set; }

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();

    public virtual SubCategory? SubCategory { get; set; }

    public virtual ICollection<SubcategoryMapping> SubcategoryMappings { get; set; } = new List<SubcategoryMapping>();
}
