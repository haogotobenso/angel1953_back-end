using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Report
{
    public string ReportId { get; set; } = null!;

    public string ReportSource { get; set; } = null!;

    public DateTime ReportTime { get; set; }

    public bool IsReport { get; set; }

    public byte Object { get; set; }

    public string Url { get; set; } = null!;

    public byte[]? Picture { get; set; }

    public bool IsBullying { get; set; }

    public string? Solution { get; set; }

    public int DetailId { get; set; }

    public int PlatformId { get; set; }

    public int TypeId { get; set; }

    public string? Account { get; set; }

    public int? SchoolId { get; set; }

    public string? AccountInfo { get; set; }

    public virtual Member? AccountNavigation { get; set; }

    public virtual ReportDetail Detail { get; set; } = null!;

    public virtual Platform Platform { get; set; } = null!;

    public virtual School? School { get; set; }

    public virtual ReportType Type { get; set; } = null!;
}
