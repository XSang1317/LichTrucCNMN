using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using System.Security.Claims;
using System.Linq;
using System.Reflection.Metadata;
using LichTruc.Utils;

namespace LichTruc.Controllers.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private AppDbContext db = null;
        public AreaController(AppDbContext db)
        {
            this.db = db;
        }
        //public interface IAreaController
        //{
        //    Task<IActionResult> GetAllAreas();
        //    Task<IActionResult> GetArea(int id);
        //    Task<IActionResult> CreateArea(LichTruc.Data.Entities.Area model);
        //    Task<IActionResult> UpdateArea(int id, LichTruc.Data.Entities.Area model);
        //    Task<IActionResult> DeleteArea(int id);
        //}
        //Get all
        //[HttpGet]
        //public IActionResult GetAreas()
        //{
        //    var areas = db.Areas.ToList();
        //    return Ok(areas);
        //}
        //public class AreaModel
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public DateTime? CreatedAt { get; set; }
        //    public DateTime? UpdatedAt { get; set; }
        //    public DateTime? DeletedAt { get; set; }
        //    public string CreatedBy { get; set; }
        //    public string UpdatedBy { get; set; }
        //}
        [HttpGet] ///Get all
        public async Task<IActionResult> GetAllAreas()
        {
            var areas = await db.Areas.ToListAsync();
            var areaModels = areas.Select(a => new LichTruc.Data.Entities.Area
            {
                Id = a.Id,
                Name = a.Name,
                code = a.code,
                Description = a.Description,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                DeletedAt = a.DeletedAt,
                CreatedBy = a.CreatedBy,
                UpdatedBy = a.UpdatedBy
            }).ToList();

            return Ok(areaModels);
        }
        //[HttpGet]
        //public IActionResult GetAllArea(String msg)
        //{
        //    var query = $"%{msg?.ToLower()}%";

        //    var areaType = db.Areas.Where(x => (string.IsNullOrEmpty(msg) ||
        //        (EF.Functions.Like(x.Name.ToLower(), query)) ||
        //        (EF.Functions.Like(x.Description.ToLower(), query))) && x.Id != Utils.Constant.BLANK_CONTRACT_TYPE_ID).
        //        Select(x => new { x.Id, x.Name, x.Description }).AsNoTracking().ToList();
        //    return Ok(areaType);
        //}
        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] LichTruc.Data.Entities.Area model)
        {
            var area = new LichTruc.Data.Entities.Area
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow,
               // CreatedBy = "",
            };

            await db.Areas.AddAsync(area);
            await db.SaveChangesAsync();

            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] LichTruc.Data.Entities.Area model)
        {
            var area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            area.Name = model.Name;
            area.Description = model.Description;
            area.UpdatedAt = DateTime.UtcNow;
            //area.UpdatedBy = "Admin";

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var area = await db.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            area.DeletedAt = DateTime.UtcNow;
            //area.UpdatedBy = "admin";

            await db.SaveChangesAsync();

            return Ok();
        }

        private bool AreaExists(int id)
        {
            return db.Areas.Any(e => e.Id == id);
        }

    }
}
