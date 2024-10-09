using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Platform
{
    public int PlatformId { get; set; }

    public string? PlatformName { get; set; }

    public int PlatformTypeId { get; set; }

    public virtual PlatformType PlatformType { get; set; } = null!;
}
