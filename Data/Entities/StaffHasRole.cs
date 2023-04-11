using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class StaffHasRole
    {
        public int staffId { get; set; }
        public Staff Staff { get; set; }
        public int roleId { get; set; }
        public Role Role { get; set; }
    }
}
