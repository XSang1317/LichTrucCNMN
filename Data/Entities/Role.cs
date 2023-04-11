using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class Role
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; } = string.Empty;

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        //default
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedAt { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


        public IList<StaffHasRole> StaffHasRoles { get; set; }
        public IList<RoleHasPermission> RoleHasPermissions { get; set; }
    }
}
