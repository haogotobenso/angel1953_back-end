using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class VideoLink
{
    public int VideoId { get; set; }

    public string VideoName { get; set; } = null!;

    public byte[]? VideoImg { get; set; }

    public string VideoLink1 { get; set; } = null!;

    public int? LinkClick { get; set; }
}
