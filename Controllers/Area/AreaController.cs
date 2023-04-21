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



     /*    //Get all
        [HttpGet, Authorize]
        public IActionResult GetAllArea()
        {
            var items = db.Areas.AsNoTracking().OrderBy(x=>x.Name).ToList();
            return Ok(items);
        }

        [HttpPost, Authorize]
        public IActionResult PostArea([FromBody] LichTruc.Data.Entities.Area model)
        {
            try
            {
                //Check code area
                var item = db.Areas.FirstOrDefault(i => i.code == model.code);
                if (item != null)
                    return BadRequest("Mã khu vực đã tồn tại");
                //Check name area
                item = db.Areas.FirstOrDefault(i => i.Name == model.Name);
                if (item != null)
                    return BadRequest("Tên khu vực đã tồn tại");

                var staffID = User.FindFirstValue(ClaimTypes.Sid);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = Convert.ToInt32(staffID);

                db.Areas.Add(model);
                db.SaveChanges();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Authorize]
        public IActionResult UpdateArea([FromBody] LichTruc.Data.Entities.Area model)
        {
            var item = db.Areas.FirstOrDefault(i => i.code == model.code && i.Id != model.Id);
            if (item != null)
                return BadRequest("Mã khu vực đã tồn tại");
            item = db.Areas.FirstOrDefault(i => i.Name == model.Name && i.Id != model.Id);
            if (item != null)
                return BadRequest("Tên khu vực đã tồn tại");
            try
            {
                var staffId = User.FindFirstValue(ClaimTypes.Sid);

                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = Convert.ToInt32(staffId);

                db.Areas.Update(model);
                db.SaveChanges();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteId (int id)
        {
            var item = db.Areas.FirstOrDefault(x => x.Id == id);

            item.DeletedAt = DateTime.Now;
            item.code = item.code + "_x_" + DateTime.Now.ToString("ddMMHHmmss");
            item.Name = item.Name + "_x_" + DateTime.Now.ToString("ddMMyyHHmmss"); //Name la duy nhat

            db.Areas.Update(item);
            db.SaveChanges();

            return Ok(item);
        } */

    }
}
