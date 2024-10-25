using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class ShowCaseDto
    {
        public int BPId { get; set; }

        public DateTime? PostTime { get; set; }

        public DateTime? FirstDate {get;set;}

        public string? Bullyinger{get; set;}

        public string FBurl{get; set;}

        public string Posturl{get;set;}
    }
}