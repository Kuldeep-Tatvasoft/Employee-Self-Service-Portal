using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class SubcategoryMapping
{
    public long SubcategoryMappingId { get; set; }

    public long? RequestId { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public virtual GroupCategory? Category { get; set; }

    public virtual HelpdeskRequest? Request { get; set; }

    public virtual SubCategory? SubCategory { get; set; }
}
