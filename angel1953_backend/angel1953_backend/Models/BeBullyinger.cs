using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class BeBullyinger
{
    public int BeBullyingerId { get; set; }

    public string BeBullyinger1 { get; set; }

    public string FBurl { get; set; }

    public string LinkToCode { get; set; }

    public string Account { get; set; }

    public virtual Member AccountNavigation { get; set; }

    public virtual ICollection<ReportDetail> ReportDetail { get; set; } = new List<ReportDetail>();
}
