using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string Class1 { get; set; }

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();
}
