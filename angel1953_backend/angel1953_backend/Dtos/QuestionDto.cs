using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
    }
}