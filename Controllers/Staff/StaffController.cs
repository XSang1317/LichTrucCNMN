using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using LichTruc.Utils;
using static LichTruc.Controllers.Areas.AreaController;
//using LichTruc.Interfaces;

namespace LichTruc.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext db = null ;
       // private readonly IAreaController _areaController; ///Call AreaName to table Area

        public StaffController(AppDbContext db)
        {
            this.db = db;
           // _areaController = areaController;
        }
        public class StaffWithAreaDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public int AreaId { get; set; }
            public string AreaName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public DateTime DeleteAt { get; set; }
            public string CreatedBy { get; set; }
            public string UpdatedBy { get; set; }
        }
        //Get all
        [HttpGet]
        public async Task<IActionResult> GetAllStaffs()
        {
            var staffs = await db.Staffs
            .Include(s => s.Area)
            .Select(s => new StaffWithAreaDto
            {
                Id = s.Id,
                Name = s.Name,
                Username = s.Username,
                Email = s.Email,
                Phone = s.Phone,
                AreaId = s.areaId,
                AreaName = s.Area.Name
            })
            .ToListAsync();

            return Ok(staffs);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewStaff(LichTruc.Data.Entities.Staff staff)
        {
            // Lấy thông tin của Area từ database sử dụng LINQ Join khi cần Join nhiều dữ liệu 
            //var area = await (from s in db.Staffs
            //                  join a in db.Areas on s.areaId equals a.Id
            //                  where s.areaId == staff.areaId
            //                  select new { a.Name }).FirstOrDefaultAsync();

            var area = db.Areas.FirstOrDefault(x => x.Id == staff.areaId);
            // Nếu không tìm thấy Area, trả về BadRequest
            if (area == null)
            {
                return BadRequest($"Cannot find Area with ID {staff.areaId}");
            }

            // Tạo mới đối tượng Staff và gán các thông tin từ DTO và Area
            var newStaff = new LichTruc.Data.Entities.Staff
            {
                Name = staff.Name,
                areaId = staff.areaId,
                areaName = area.Name, // Gán AreaName từ thông tin của Area
                CreatedAt = DateTime.Now,
                CreatedBy = staff.Name,
            };

            // Lưu đối tượng Staff vào database
            db.Staffs.Add(newStaff);
            await db.SaveChangesAsync();

            // Trả về thông tin của Staff đã được tạo mới
            return Ok(newStaff);
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateStaff([FromBody] LichTruc.Data.Entities.Staff model)
        //{
        //    var staff = new LichTruc.Data.Entities.Staff
        //    {
        //        Name = model.Name,
        //        Username = model.Username,
        //        Email = model.Email,
        //        password = model.password,
        //        Phone = model.Phone,
        //        //RefreshToken = model.RefreshToken,
        //        salt = model.salt,
        //        CreatedAt = DateTime.UtcNow,
        //        CreatedBy = "1",
        //        areaId = model.areaId
        //    };

        //    await db.Staffs.AddAsync(staff);
        //    await db.SaveChangesAsync();

        //    return Ok();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, LichTruc.Data.Entities.Staff staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }

            db.Entry(staff).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] LichTruc.Data.Entities.Staff model)
        {
            var staff = await db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            staff.Name = model.Name;
            staff.Username = model.Username;
            staff.Email = model.Email;
            staff.password = model.password;
            staff.Phone = model.Phone;
            staff.RefreshToken = model.RefreshToken;
            staff.salt = model.salt;
            staff.UpdatedAt = DateTime.UtcNow;
            staff.UpdatedBy = "admin";
            staff.areaId = model.areaId;

            await db.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            db.Staffs.Remove(staff);
            await db.SaveChangesAsync();

            return Ok();
        }


        private bool StaffExists(int id)
        {
            return db.Staffs.Any(e => e.Id == id);
        }
    }
}
