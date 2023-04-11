using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using System.Security.Claims;

namespace LichTruc.Controllers.ShiftType
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsTypeController : ControllerBase
    {
        private AppDbContext db = null;
        public ShiftsTypeController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet, Authorize]
        public IActionResult GetAllType()
        {
            var items = db.Types.AsNoTracking().OrderBy(x=>x.name).ToList();
            return Ok(items);
        }
        [HttpPost, Authorize]
        public IActionResult PostShiftType([FromBody] ShiftsType type)
        {
            try
            {
                var item = db.Types.FirstOrDefault(i => i.name == type.name);
                if (item != null)
                {
                    return BadRequest("Tên ca làm đã tồn tại");
                }
                var staffId = User.FindFirstValue(ClaimTypes.Sid);
                type.CreatedAt = DateTime.Now;
                type.CreatedBy = Convert.ToInt32(staffId);

                db.Types.Add(type);
                db.SaveChanges();
                return Ok(type);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut, Authorize]
        public IActionResult UpdateShiftType([FromBody] ShiftsType type)
        {
            var item = db.Types.FirstOrDefault(i => i.name == type.name && i.id == type.id);
            if (item != null)
            {
                return BadRequest("Tên ca làm đã tồn tại");
            }
            try
            {
                var staffId = User.FindFirstValue(ClaimTypes.Sid);

                type.UpdatedAt = DateTime.Now;
                type.UpdatedBy = Convert.ToInt32(staffId);

                db.Types.Update(item);
                db.SaveChanges();

                return Ok(type);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteId(int id)
        {

            var item = db.Types.FirstOrDefault(x => x.id == id);

            item.DeletedAt = DateTime.Now;
            item.name = item.name + "_x_" + DateTime.Now.ToString("ddMMyyHHmmss"); //Name la duy nhat

            db.Types.Update(item);
            db.SaveChanges();

            return Ok(item);
        }
    }
}
