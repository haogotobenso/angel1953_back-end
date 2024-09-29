using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Platform
{
    public int PlatformId { get; set; }

    public string PlatformName { get; set; } = null!;

    public int PlatformTypeId { get; set; }

    public int ReportTypeId { get; set; }

    public virtual PlatformType PlatformType { get; set; } = null!;

    public virtual ICollection<Report> Report { get; set; } = new List<Report>();

    public virtual ReportType ReportType { get; set; } = null!;
}
