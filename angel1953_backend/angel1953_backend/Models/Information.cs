using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Information
{
    public int InformationId { get; set; }

    public string Information1 { get; set; } = null!;

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
