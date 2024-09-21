using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class ReportType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Platform> Platform { get; set; } = new List<Platform>();

    public virtual ICollection<Report> Report { get; set; } = new List<Report>();
}
