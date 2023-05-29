namespace LichTruc.Data.Entities
{
    public class StaffHasArea
    {
        public int staffId { get; set; }
        public Staff Staff { get; set; }

        public int areaId { get; set; }
        public Area Area { get; set; }
    }
}
