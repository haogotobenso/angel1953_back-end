using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class AnswerDto
    {
        public int QuestionId{ get; set; }
        public string UserAnswer{get; set;}
    }
}