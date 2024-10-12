using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class BullyingerPost
{
    public int BPId { get; set; }

    public string? BullyingerId { get; set; }

    public string PostInfo { get; set; } = null!;

    public DateTime? PostTime { get; set; }

    public string? Posturl { get; set; }

    public string? KeyWord { get; set; }

    public virtual Bullyinger? Bullyinger { get; set; }
}
