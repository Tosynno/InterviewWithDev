using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class tbl_RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpire { get; set; }
        public Guid StaffId { get; set; }
        public tbl_StaffBioData tblStaffBioData { get; set; }
    }
}
