using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace LichTruc.Data.Entities
{
    public class PickListMenu
    {
        public int id { get; set; }
        public string PickListType { get; set; } = string.Empty;
        public string PickListName { get; set; } = string.Empty;

        //Default
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
