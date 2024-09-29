using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class ExternalLinks
{
    public int LinkId { get; set; }

    public string Title { get; set; } = null!;

    public string Link { get; set; } = null!;

    public DateTime LinkTime { get; set; }
}
