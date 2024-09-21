using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string SchoolName { get; set; }

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();

    public virtual ICollection<Report> Report { get; set; } = new List<Report>();
}
