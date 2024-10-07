﻿using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class BullyingDetect
{
    public int Id { get; set; }

    public int BullyingerId { get; set; }

    public byte State { get; set; }

    public DateTime Date { get; set; }

    public bool Process { get; set; }

    public virtual Bullyinger Bullyinger { get; set; } = null!;
}