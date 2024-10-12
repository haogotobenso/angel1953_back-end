using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string? Question1 { get; set; }

    public string? Answer { get; set; }

    public string? Option1 { get; set; }

    public string? Option2 { get; set; }

    public string? Option3 { get; set; }

    public virtual ICollection<RecoveryRecord> RecoveryRecord { get; set; } = new List<RecoveryRecord>();
}
