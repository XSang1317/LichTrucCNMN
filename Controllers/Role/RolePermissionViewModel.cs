namespace LichTruc.Controllers.Role
{
    public class RolePermissionViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool HasPermission { get; set; }
        public int? PermissionGroupId { get; set; }
    }
}
