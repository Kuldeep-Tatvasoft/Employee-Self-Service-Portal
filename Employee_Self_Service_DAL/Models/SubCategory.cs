using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class SubCategory
{
    public int SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public int? CategoryId { get; set; }

    public virtual GroupCategory? Category { get; set; }

    public virtual ICollection<HelpdeskRequest> HelpdeskRequests { get; set; } = new List<HelpdeskRequest>();

    public virtual ICollection<SubcategoryMapping> SubcategoryMappings { get; set; } = new List<SubcategoryMapping>();
}
