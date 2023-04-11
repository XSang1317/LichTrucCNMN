using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace LichTruc.Data.Entities
{
    public class PickList
    {
        public int id { get; set; }
        public string PickListType { get; set; } = string.Empty;
        public string PickListValue { get; set; } = string.Empty;

        public int Parent { get; set; }
        public int SortOrder { get; set; }
        
        public bool? Used { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletedAt { get; set; }

        public int? CreateBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
