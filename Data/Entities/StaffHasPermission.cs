using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class StaffHasPermission
    {
        public int staffId { get; set; }
        public Staff? Staff { get; set; }

        public int permissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
