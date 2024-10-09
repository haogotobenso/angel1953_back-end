using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Bullyinger
{
    public int BullyingerId { get; set; }

    public string Bullyinger1 { get; set; } = null!;

    public int postTimes { get; set; }

    public int pointTimes { get; set; }

    public string Account { get; set; } = null!;

    public virtual Member AccountNavigation { get; set; } = null!;

    public virtual ICollection<BullyingDetect> BullyingDetect { get; set; } = new List<BullyingDetect>();
}
