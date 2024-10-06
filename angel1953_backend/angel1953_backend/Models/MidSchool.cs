using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class MidSchool
{
    public int MidSchoolId { get; set; }

    public string MidSchool1 { get; set; } = null!;

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();
}
