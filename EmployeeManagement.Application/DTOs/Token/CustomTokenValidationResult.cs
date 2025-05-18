using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTOs.Token
{
    public class CustomTokenValidationResult
    {
        public bool Valid { get; set; }
        public TokenUserInfo? User { get; set; }
    }
}
