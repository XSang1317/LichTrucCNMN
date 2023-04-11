using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class ShiftsType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = "";
        [NotMapped]
        public TimeOnly timestar { get; set; }// giờ tối thiểu
        [NotMapped]
        public TimeOnly timeend { get; set; } // giờ tối đa

        //Foreign Key
        public virtual ICollection<Shifts>? Shifts { get; set; }
        //Defaults
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
