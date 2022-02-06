using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class tbl_StaffBioData
    {
        public Guid Id { get; set; }
        public string StaffName { get; set; }
        public string StaffNo { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<tbl_RefreshToken> tblRefreshToken { get; set; }
    }
}
