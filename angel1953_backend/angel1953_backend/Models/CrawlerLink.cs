using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class CrawlerLink
{
    public int LinkId { get; set; }

    public string LinkName { get; set; } = null!;

    public string FBLink { get; set; } = null!;

    public string Account { get; set; } = null!;
}
