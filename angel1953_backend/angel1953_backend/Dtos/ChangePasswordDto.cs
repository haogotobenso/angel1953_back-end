using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angel1953_backend.Dtos
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordCheck { get; set; }
        
    }
}