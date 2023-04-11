using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class Shifts
    {
        [Key]
        public int Id { get;set; }
        public int? typeId { get; set; }
        public int? areaId { get; set; }
        public int? staffId { get; set; }
        public int qualitymin { get; set; }
        public int qualitymax { get; set; }

        //Forgin Key
        public Staff? Staff { get; set; }
        public Area? Area { get; set; }
        public ShiftsType? ShiftsType { get; set; }

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
