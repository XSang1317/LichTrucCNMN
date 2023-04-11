using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using LichTruc.Utils;

namespace LichTruc.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private AppDbContext db = null;
        public StaffsController(AppDbContext db)
        {
            this.db = db;
        }
        [HttpGet,Authorize(Roles ="crud_user")]
        public IActionResult GetAllStaff(string q) //Get All Staff
        {
            var Query = $"%{q?.ToLower()}%";
            
            var staff = db.Staffs.Include(x => x.StaffHasRoles).ThenInclude(x => x.Role)
                                      .Where(x => x.DeletedAt == null && (string.IsNullOrEmpty(q) 
                                        || (EF.Functions.Like(x.Username.ToLower(),Query)
                                        || (EF.Functions.Like(x.Name.ToLower(), Query)))))
                                      .Select(x=> new {x.Id, x.Username,x.Name, x.Email, x.Phone, role_name = GetRoleName(x.StaffHasRoles), x.status})
                                      .AsNoTracking().ToList();
            return Ok(staff);
        }
        [HttpPost, Authorize(Roles = "crud_user")]
        public IActionResult CreateNewStaff([FromBody] LichTruc.Data.Entities.Staff staff) //Add New Staff
        {
            try
            {
                var check_for_exist = db.Staffs.FirstOrDefault(x => x.Username == staff.Username && x.DeletedAt == null);
                if (check_for_exist != null) return BadRequest("Tài khoản đã tồn tại");

                var passwordHash = AuthUtils.hashPassword(staff.password);

                var currentPath = Directory.GetCurrentDirectory();
                ///Add Image///
                ///
                //var imageFoler = Path.Combine(currentPath, "clientapp\\dist\\image\\users");
                //var imageTempFoler = Path.Combine(currentPath, "clientapp\\dist\\image\\temp");

                //if (!string.IsNullOrWhiteSpace(staff.signature))
                //{
                //    var fileExtension = staff.signature.Split('.').Last();
                //    var ImageTempName = staff.signature.Split('\\').Last();
                //    var ImageAccountName = staff.account + "." + DateTime.Now.Ticks + "." + fileExtension;
                //    var filePath = Path.Combine(imageFoler, ImageAccountName);
                //    var fileTempPath = Path.Combine(imageTempFoler, ImageTempName);

                //    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                //    staff.signature = "\\image\\users\\" + ImageAccountName;

                //    System.IO.File.Copy(fileTempPath, filePath, true);
                //}
                //else
                //{
                //    staff.signature = "";
                //}
                staff.Username = staff.Username.ToLower();
                staff.salt = passwordHash.salt;
                staff.password = passwordHash.hashed;
                staff.CreatedAt = DateTime.Now;

                db.Staffs.Add(staff);
                db.SaveChanges();

                return Ok(staff);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{slug}")]
        public IActionResult Show (string slug)
        {
            var item = db.Staffs
                         .Include(x => x.StaffHasRoles).ThenInclude(x=>x.Role)
                         .Where(i=> i.Username == slug && i.DeletedAt == null)
                         .AsEnumerable()
                         .Select(x=> new {x.Id, x.Username, x.Email,x.Phone, x.status, rolo_id = GetRoleID(x.StaffHasRoles)})
                         .FirstOrDefault();
            return Ok(item);
        }
        [HttpGet("byid/{id}")]
        public IActionResult GetById(int id)
        {
            var item = db.Staffs.Where(x => x.Id == id && x.DeletedAt == null)
                                .Select(x=> new {x.Id, x.Username,x.Name,x.Email,x.Phone})
                                .FirstOrDefault();
            return Ok(item);
        }
        public class ChangPasswordPayLoad
        {
            public int id { get; set; }
            public string old_password { get; set; }
            public string new_password { get; set; }
        }
        [HttpPut("changepassword")]
        public IActionResult ChangePasswordById([FromBody] ChangPasswordPayLoad model)
        {
            try
            {
                var item = db.Staffs.FirstOrDefault(x => x.Id == model.id && x.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(model.new_password) && !string.IsNullOrWhiteSpace(model.old_password))
                {
                    var new_passwordHash = AuthUtils.hashPassword(model.new_password);
                    var old_passwordHash = AuthUtils.hashPassword(model.old_password);

                    if(AuthUtils.checkPasswordHash(model.old_password, item.salt, item.password))
                    {
                        item.salt = new_passwordHash.salt;
                        item.password = new_passwordHash.hashed;
                        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        item.RefreshToken = refreshToken;
                        item.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        return BadRequest("Mật khẩu cũ nhập vào không trùng nhau!");
                    }
                }
                db.Staffs.Update(item);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class AccountPayLoad
        {
            public int id { get; set; }
            public string name { get; set; }
            public string password { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public int status { get; set; }
            public int UpdatedBy { get; set; }
        }
        [HttpPut, Authorize( Roles = "crud_user")]
        public IActionResult UpdateAccount([FromBody] AccountPayLoad model)
        {
            try
            {
                var item = db.Staffs.Include(x => x.StaffHasRoles).FirstOrDefault(i => i.Id == model.id && i.DeletedAt == null);
                if(item != null)
                {
                    if (!string.IsNullOrWhiteSpace(model.password))
                    {
                        var passwordHash = AuthUtils.hashPassword(model.password);
                        item.salt = passwordHash.salt;
                        item.password = passwordHash.hashed;
                        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        item.RefreshToken = refreshToken;
                    }
                    if (!string.IsNullOrWhiteSpace(model.name))
                    {
                        item.Name = model.name;
                    }
                    item.Phone = model.phone;
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = model.UpdatedBy;

                    //Image Add
                    //if (!item.signature.Equals(model.signature))
                    //{
                    //    var currentPath = Directory.GetCurrentDirectory();
                    //    var imageFoler = Path.Combine(currentPath, "clientapp\\dist\\image\\users");
                    //    var imageTempFoler = Path.Combine(currentPath, "clientapp\\dist\\image\\temp");

                    //    //Xóa file image cũ
                    //    Directory.CreateDirectory(imageFoler);
                    //    System.IO.DirectoryInfo di2 = new DirectoryInfo(imageFoler);
                    //    foreach (FileInfo temp in di2.GetFiles())
                    //    {
                    //        if (temp.Name.Split('.').First().Equals(item.account))
                    //            temp.Delete();
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(model.signature))
                    //    {
                    //        var fileExtension = model.signature.Split('.').Last();
                    //        var ImageTempName = model.signature.Split('\\').Last();
                    //        var ImageAccountName = item.account + "." + DateTime.Now.Ticks + "." + fileExtension;
                    //        var filePath = Path.Combine(imageFoler, ImageAccountName);
                    //        var fileTempPath = Path.Combine(imageTempFoler, ImageTempName);

                    //        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    //        item.signature = "\\image\\users\\" + ImageAccountName;

                    //        System.IO.File.Copy(fileTempPath, filePath, true);
                    //    }
                    //    else
                    //    {
                    //        item.signature = "";
                    //    }
                    //}
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}"), Authorize(Roles = "crud_user")]
        public async Task<IActionResult> Delete(int id, int UpdatedBy)
        {
            try
            {
                var item = db.Staffs.Include(x => x.StaffHasRoles).FirstOrDefault(i => i.Id == id && i.DeletedAt == null);
                if (item != null)
                {
                    item.DeletedAt = DateTime.Now;
                    item.UpdatedBy = UpdatedBy;
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static string GetRoleName(IList<StaffHasRole> staffRoles)
        {
            string result = string.Empty;
            foreach(var staffRole in staffRoles)
            {
                if (!string.IsNullOrWhiteSpace(result))
                    result += ", ";
                result += staffRole.Role.name;
            }
            return result;
        }
        private static string GetRoleID(IList<StaffHasRole> staffRoles)
        {
            string result = string.Empty;
            foreach(var staffRole in staffRoles)
            {
                if (!string.IsNullOrWhiteSpace (result))
                    result += ", ";
                result += staffRole.Role.id;
            }
            return result;
        }
    }
}
