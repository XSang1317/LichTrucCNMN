namespace LichTruc.Controllers.Staff
{
    public class UpdateStaffViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public List<int> selectedRoles { get; set; }
    }
}
