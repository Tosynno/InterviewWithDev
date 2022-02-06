using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AccountResponse
    {
        public string StaffName { get; set; }
        public AuthResultDto Token { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseCode { get; set; }
    }
}

