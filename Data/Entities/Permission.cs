using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class Permission
    {
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string? code { get; set; }
        [Required]
        [StringLength(255)]
        public string? name { get; set; }
        [Required]
        [StringLength(255)]
        public string? description { get; set; }

        //Default
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        //FK
        public int? PermissionGroupId { get; set; }
        public PermissionGroup? PermissionGroup { get; set; }

        //
        public IList<RoleHasPermission>? RoleHasPermissions { get; set; }
    }
}
