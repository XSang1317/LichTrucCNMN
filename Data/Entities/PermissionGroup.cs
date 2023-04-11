using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class PermissionGroup
    {
        public int id { get; set; }
        public int? parent { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; } = "";

        //
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        public Boolean hasPermission { get; set; }

        public IList<Permission>? Permissions { get; set; }
    }
}
