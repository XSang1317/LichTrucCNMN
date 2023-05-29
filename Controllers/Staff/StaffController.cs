using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using LichTruc.Utils;
using static LichTruc.Controllers.Areas.AreaController;
using Microsoft.EntityFrameworkCore.Storage;
//using LichTruc.Interfaces;

namespace LichTruc.Controllers.Staff
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // Lấy danh sách Staff
        public async Task<List<Data.Entities.Staff>> GetStaffListAsync()
        {
            var query = from s in _context.Staffs
                        join a in _context.Areas on s.areaId equals a.Id into areaJoin
                        from area in areaJoin.DefaultIfEmpty()
                            //join r in db.Roles on s.roleId equals r.id into roleJoin
                            //from role in roleJoin.DefaultIfEmpty()
                        select new Data.Entities.Staff
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Username = s.Username,
                            Email = s.Email,
                            password = s.password,
                            Phone = s.Phone,
                            areaId = s.areaId,
                            //  roleId = s.roleId,
                            CreatedAt = s.CreatedAt,
                            UpdatedAt = s.UpdatedAt,
                            DeletedAt = s.DeletedAt,
                            areaName = area.Name, // Lấy dữ liệu AreaName
                                                  //    RoleName = role.name // Lấy dữ liệu RoleName
                        };

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }
        //public class StaffDto
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string Username { get; set; }
        //    public string Email { get; set; }
        //    public string Phone { get; set; }
        //    public string password { get; set; }
        //    public DateTime CreatedAt { get; set; }
        //    public DateTime UpdatedAt { get; set; }
        //    public int AreaId { get; set; }
        //    public string? AreaName { get; set; }
        //    public AreaDto? AreaDto { get; set; }
        //}

        //public class AreaDto
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public string? Code { get; set; }
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateStaff(Data.Entities.Staff staff)
        //{
        //    try
        //    {
        //        var check_exist = _context.Staffs.FirstOrDefault(x => x.Username == staff.Username && x.DeletedAt == null);
        //        if (check_exist != null)
        //        {
        //            return BadRequest("Tài khoản đã tồn tại !");
        //        }
        //        var area = await _context.Areas.FirstOrDefaultAsync(a => a.Name == staff.areaName && a.DeletedAt == null);
        //        if (area == null)
        //        {
        //            return BadRequest("Invalid areaName");
        //        }
        //        var passwordHash = AuthUtils.hashPassword(staff.password);
        //        var currentPath = Directory.GetCurrentDirectory();

        //        staff.Username = staff.Username.ToLower();
        //        staff.salt = passwordHash.salt;
        //        staff.password = passwordHash.hashed;
        //        staff.CreatedAt = DateTime.Now;
        //        staff.areaName = area.Name;
        //        staff.areaId = area.Id; // Lấy mã ID của khu vực tương ứng
        //        _context.Staffs.Add(staff);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(nameof(GetStaff), new { id = staff.Id }, staff);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] StaffViewModel model)
        {
            // Kiểm tra xem Id của phòng ban có hợp lệ hay không
            var area = await _context.Areas.FindAsync(model.AreaId);
            if (area == null)
            {
                return BadRequest("Phòng ban không hợp lệ");
            }
            var passwordHash = AuthUtils.hashPassword(model.Password);
            // Tạo một bản ghi mới trong bảng Staffs
            var staff = new Data.Entities.Staff
            {

                Name = model.Name,
                Username = model.UserName,
                Email = model.Email,
                Phone = model.Phone,
                salt = passwordHash.salt,
                password = passwordHash.hashed,
                areaId = model.AreaId,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return Ok("Thêm nhân viên mới thành công");
        }
        public class StaffViewModel
        {
            public string Name { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
            public int AreaId { get; set; }
        }
        //    [HttpPost]
        //    public async Task<ActionResult<Data.Entities.Staff>> CreateStaff(CreateStaffRequest request)
        //    {
        //        var staff = new Data.Entities.Staff
        //        {
        //            Name = request.Name,
        //            areaId = request.AreaId
        //        };
        //        _context.Staffs.Add(staff);
        //        await _context.SaveChangesAsync();

        //        return staff;
        //    }

        //public class CreateStaffRequest
        //{
        //    public string Name { get; set; }
        //    public int AreaId { get; set; }
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(int id, Data.Entities.Staff staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }
            var passwordHash = AuthUtils.hashPassword(staff.password);

            var area = await _context.Areas.FindAsync(staff.areaId);
            if (area == null)
            {
                return BadRequest("Invalid areaId");
            }
            staff.salt = passwordHash.salt;
            staff.password = passwordHash.hashed;

            staff.areaName = area.Name;
            _context.Entry(staff).State = EntityState.Modified;
            staff.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
