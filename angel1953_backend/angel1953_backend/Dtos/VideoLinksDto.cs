using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class VideoLinksDto
    {
        public int VideoId { get; set; }

        public string VideoName { get; set; } = null!;

        public string? ImgInnerUrl { get; set; }

        public string VideoLink1 { get; set; } = null!;

        public int? LinkClick { get; set; }
    }
}