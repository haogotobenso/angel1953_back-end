using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class PlatformType
{
    public int PlatformTypeId { get; set; }

    public string PlatformTypeName { get; set; } = null!;

    public virtual ICollection<Platform> Platform { get; set; } = new List<Platform>();
}
