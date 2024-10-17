using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class ChartDto
    {
        public string[] Labels{get; set;}
        public ChartDatasetDto[] Datasets{get;set;}
    }
    public class ChartDatasetDto
    {
        public string Label { get; set; }
        public int[] Data { get; set; }
        public int BorderWidth { get; set; }
    }
}