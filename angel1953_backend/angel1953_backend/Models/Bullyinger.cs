using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Bullyinger
{
    public string BullyingerId { get; set; } = null!;

    public string Bullyinger1 { get; set; } = null!;

    public string FBurl { get; set; } = null!;

    public byte? State { get; set; }

    public DateTime? FirstDate { get; set; }

    public int BullyingerPoint { get; set; }

    public string? Account { get; set; }

    public virtual Member? AccountNavigation { get; set; }

    public virtual ICollection<BullyingerPost> BullyingerPost { get; set; } = new List<BullyingerPost>();
}
