﻿using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string School1 { get; set; } = null!;

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();
}
