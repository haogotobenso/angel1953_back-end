using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class ReportDetail
{
    public int DetailId { get; set; }

    public int? BullyingerId { get; set; }

    public string Content { get; set; }

    public DateTime ContentTime { get; set; }

    public int? BeBullyingerId { get; set; }

    public virtual BeBullyinger BeBullyinger { get; set; }

    public virtual Bullyinger Bullyinger { get; set; }

    public virtual ICollection<Report> Report { get; set; } = new List<Report>();
}
