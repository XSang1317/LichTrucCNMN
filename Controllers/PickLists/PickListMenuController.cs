using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LichTruc.Data;

namespace LichTruc.Controllers.PickLists
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickListMenuController : ControllerBase
    {
        private readonly AppDbContext db;

        public PickListMenuController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet,Authorize]
        public IActionResult Get()
        {
            var items = db.PickListMenus.Select(x=> new {
                id =x.id, 
                PickListType = x.PickListType, 
                PickListName = x.PickListName
            })
                .ToList();
            return Ok(items);
        }
    }
}
