using System;
using System.Collections.Generic;

namespace Employee_Self_Service_DAL.Models;

public partial class Document
{
    public long DocumentId { get; set; }

    public string? Documents { get; set; }

    public long? EventId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Event? Event { get; set; }
}
