using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class ExternalLinks
{
    public int LinkId { get; set; }

    public string? Title { get; set; }

    public string? Link { get; set; }

    public DateTime? LinkTime { get; set; }
}
