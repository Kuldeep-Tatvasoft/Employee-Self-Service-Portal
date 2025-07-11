using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class GroupCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? GroupId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<HelpdeskRequest> HelpdeskRequests { get; set; } = new List<HelpdeskRequest>();

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    public virtual ICollection<SubcategoryMapping> SubcategoryMappings { get; set; } = new List<SubcategoryMapping>();
}
