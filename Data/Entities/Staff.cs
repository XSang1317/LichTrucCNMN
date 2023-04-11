using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        [StringLength(50)]
        public string Name { get; set; }
        //[Required]
        [StringLength(50)]
        public string Username { get; set; }
        public string Email { get; set; } 
        public string signature { get; set; }
        public string Phone { get; set; } 
        public string Note { get; set; } 
        public int status { get; set; } = 1;
        public string? RefreshToken { get; set; }
        [StringLength(255)]
        public string salt { get; set; } 

        [StringLength(255)]
        public string password { get; set; } 
        public int areaId { get; set; }       
        public string areaName { get; set; }
        public virtual Area Area { get; set; } //FK to Area
        public virtual ICollection<Shifts>? Shifts { get; set; } //Thuoc tinh dieu huong

        //Defaults
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        //Link FK
        public IList<StaffHasRole>? StaffHasRoles { get; set; }
        public IList<StaffHasPermission>? StaffHasPermissions { get; set; }


    }
}
