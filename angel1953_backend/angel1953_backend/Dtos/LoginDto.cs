using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class LoginDto
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}