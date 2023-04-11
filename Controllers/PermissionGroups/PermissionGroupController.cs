using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LichTruc.Data;

namespace LichTruc.Controllers.PermissionGroups
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionGroupController : ControllerBase
    {
        private AppDbContext db = null;
        public PermissionGroupController(AppDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = db.PermissionGroups.OrderBy(x => x.name).ToList();
            return Ok(new { items });
        }
    }
}
