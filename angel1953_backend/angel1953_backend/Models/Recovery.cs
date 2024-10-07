﻿using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Recovery
{
    public int RecoveryId { get; set; }

    public DateTime Time { get; set; }

    public string Account { get; set; } = null!;

    public int Correct { get; set; }

    public virtual Member AccountNavigation { get; set; } = null!;

    public virtual ICollection<RecoveryRecord> RecoveryRecord { get; set; } = new List<RecoveryRecord>();
}