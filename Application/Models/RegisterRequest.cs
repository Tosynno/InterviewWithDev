using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RegisterRequest
    {
        public string StaffName { get; set; }
       // public string StaffNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get;  set; }
    }
}
