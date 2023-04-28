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

        //Get all
        //[HttpGet]
        //public IActionResult GetAreas()
        //{
        //    var areas = db.Areas.ToList();
        //    return Ok(areas);
        //}

        //[HttpGet] ///Get all
        //public async Task<IActionResult> GetAllAreas()
        //{
        //    var areas = await db.Areas.ToListAsync();
        //    var areaModels = areas.Select(a => new LichTruc.Data.Entities.Area
        //    {
        //        Id = a.Id,
        //        Name = a.Name,
        //        code = a.code,
        //        Description = a.Description,
        //        CreatedAt = a.CreatedAt,
        //        UpdatedAt = a.UpdatedAt,
        //        DeletedAt = a.DeletedAt,
        //        CreatedBy = a.CreatedBy,
        //        UpdatedBy = a.UpdatedBy
        //    }).ToList();

        //    return Ok(areaModels);
        //}
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
        }
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreAreaViewModel model)
        {
            try
            {
                var item = db.Areas.FirstOrDefault(i => i.Name == model.name &&  i.Description == model.description);
               
                if (item != null)
                    return BadRequest("Group name already exists");
                item = new Data.Entities.Area()
                {
                    Name = model.name,
                    Description = model.description,
                    code = "",
                    CreatedAt = DateTime.Now,
                    CreatedBy = model.CreatedBy,
                };
                db.Areas.Add(item);
                db.SaveChanges();
                return Ok(new { item });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        //[HttpPost]
        //public IActionResult PostArea([FromBody] LichTruc.Data.Entities.Area model)
        //{
        //    try
        //    {
        //        //Check code area
        //        var item = db.Areas.FirstOrDefault(i => i.code == model.code);
        //        if (item != null)
        //            return BadRequest("Mã khu vực đã tồn tại");
        //        //Check name area
        //        item = db.Areas.FirstOrDefault(i => i.Name == model.Name);
        //        if (item != null)
        //            return BadRequest("Tên khu vực đã tồn tại");
        //        model.Description = model.Description;
        //        model.CreatedAt = DateTime.Now;
        //        //model.CreatedBy = 

        //        db.Areas.Add(model);
        //        db.SaveChanges();
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
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
        // DELETE: api/Areas/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteArea(int id)
        //{
        //    var area = await db.Areas.FindAsync(id);
        //    if (area == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Areas.Remove(area);
        //    await db.SaveChangesAsync();

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
