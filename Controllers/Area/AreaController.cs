using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using System.Security.Claims;
using System.Linq;

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
        }
    }
}
