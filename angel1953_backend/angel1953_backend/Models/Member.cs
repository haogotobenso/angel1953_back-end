using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Member
{
    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AuthCode { get; set; }

    public bool IsTeacher { get; set; }

    public int SchoolId { get; set; }

    public byte[]? TeacherImg { get; set; }

    public int? ClassId { get; set; }

    public string? StudentId { get; set; }

    public virtual ICollection<BeBullyinger> BeBullyinger { get; set; } = new List<BeBullyinger>();

    public virtual ICollection<Bullyinger> Bullyinger { get; set; } = new List<Bullyinger>();

    public virtual Class? Class { get; set; }

    public virtual ICollection<Recovery> Recovery { get; set; } = new List<Recovery>();

    public virtual ICollection<Report> Report { get; set; } = new List<Report>();

    public virtual School School { get; set; } = null!;
}
