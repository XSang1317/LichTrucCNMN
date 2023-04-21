using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 


        //Defaults
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        //Foreign Key
        public ICollection<Staff>? Staff { get; set; }
        public virtual ICollection<Shifts>? Shifts { get; set; }

    }
}
