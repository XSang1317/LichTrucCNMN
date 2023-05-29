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

        // GET: api/Areas
        [HttpGet]
        public IActionResult GetAreas()
        {
            var areas = db.Areas.ToList();
            return Ok(areas);
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Data.Entities.Area>> GetArea(int id)
        {
            var area = await db.Areas.FindAsync(id);

            if (area == null)
            {
                return NotFound();
            }

            return area;
        }

        // POST: api/Areas
        //[HttpPost]
        //public IActionResult CreateArea(Data.Entities.Area area)
        //{
        //    db.Areas.Add(area);
        //    db.SaveChanges();
        //    return CreatedAtAction(nameof(GetAreas), new { id = area.Id }, area);
        //}
        public class StoreAreaViewModel
        {
            public string? name { get; set; }
            public string? description { get; set; }
            public string? code { get; set; }
            public int CreatedBy { get; set; }
            public string? CreatedAt { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreAreaViewModel model)
        {
            try
            {
                var item = db.Areas.FirstOrDefault(i => i.code == model.code);
                if (item != null)
                    return BadRequest("Mã khu vực đã tồn tại");
                item = db.Areas.FirstOrDefault(i => i.Name == model.name);
                if (item != null)
                    return BadRequest("Tên khu vực đã tồn tại");
                item = new Area()
                {
                    Name = model.name,
                    Description = model.description,
                    code = model.code,
                    CreatedAt = DateTime.Now,
                    CreatedBy = model.CreatedBy,
                };
                db.Areas.Add(item);
                db.SaveChanges();
                return Ok(new { item });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        // PUT: api/Areas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(int id, Data.Entities.Area area)
        {
            if (id != area.Id)
            {
                return BadRequest();
            }

            db.Entry(area).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //[HttpPut("{id}")]
        //public IActionResult UpdateArea(int id, Data.Entities.Area area)
        //{
        //    var areaToUpdate = db.Areas.Find(id);
        //    if (areaToUpdate == null)
        //        return NotFound();
        //    areaToUpdate.Name = area.Name;
        //    areaToUpdate.Description = area.Description;
        //    db.Areas.Update(areaToUpdate);
        //    db.SaveChanges();
        //    return NoContent();
        //}
        [HttpDelete("{id}")]
        public IActionResult DeleteArea(int id)
        {
            var areaToDelete = db.Areas.Find(id);
            if (areaToDelete == null)
                return NotFound();
            db.Areas.Remove(areaToDelete);
            db.SaveChanges();
            return NoContent();
        }

        private bool AreaExists(int id)
        {
            return db.Areas.Any(e => e.Id == id);
        }
    }
}
