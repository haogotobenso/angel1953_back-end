using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class RecoveryRecord
{
    public int RecordId { get; set; }

    public int RecoveryId { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; }

    public virtual Recovery Recovery { get; set; }
}
