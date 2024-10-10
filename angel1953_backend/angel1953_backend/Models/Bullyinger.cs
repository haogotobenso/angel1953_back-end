using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Bullyinger
{
    public int BullyingerId { get; set; }

    public string FBurl { get; set; } = null!;

    public byte State { get; set; }

    public DateTime FirstDate { get; set; }

    public int BullyingerPoint { get; set; }

    public string? Account { get; set; }

    public virtual Member? AccountNavigation { get; set; }
}
