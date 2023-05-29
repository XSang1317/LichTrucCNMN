using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;

namespace LichTruc.Controllers.Role
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private AppDbContext db = null;
        public RoleController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        //public IActionResult GetRoles()
        //{
        //    var roles = db.Roles.ToList();
        //    return Ok(roles);
        //}
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await db.Roles.ToListAsync();
            var rolesModel = roles.Select(a => new LichTruc.Data.Entities.Role
            {
                id = a.id,
                name = a.name,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                DeletedAt = a.DeletedAt,
                CreatedBy = a.CreatedBy,
                UpdatedBy = a.UpdatedBy
            }).ToList();

            return Ok(rolesModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetRoles(string q)
        //{
        //    var query = $"%{q?.ToLower()}%";

        //    var items = await db.Roles
        //        .Where(x => string.IsNullOrEmpty(q) || EF.Functions.Like(x.Name.ToLower(), query))
        //        .Select(x => new { x.Id, x.Name })
        //        .OrderBy(x => x.Name)
        //        .ToListAsync();

        //    return Ok(new { items });
        //}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await db.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(new { role });
        }
        [HttpGet]
        [Route("{id?}/staff")]
        public async Task<IActionResult> GetUserByRole(int? id, string userSearchQuery)
        {
            try
            {
                var Query = $"%{userSearchQuery?.ToLower()}%";

                var role = db.Roles.Include(x => x.StaffHasRoles).ThenInclude(x => x.Staff).FirstOrDefault(x => x.id == id);
                //var items = role.UserRoles.Select(x => x.User).ToList;
                var items = role.StaffHasRoles
                    .Where(x => string.IsNullOrEmpty(userSearchQuery) || (EF.Functions.Like(x.Staff.Username.ToLower(), Query))
                                                                    || (EF.Functions.Like(x.Staff.Name.ToLower(), Query)
                                                                    || (EF.Functions.Like(x.Staff.Email.ToLower(), Query))))
                    .Select(x => new { x.Staff.Id, x.Staff.Username, x.Staff.Email, x.Staff.Name }).ToList();
                return Ok(new { items });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreRoleViewModel model)
        {
            try
            {
                var item = db.Roles.FirstOrDefault(i => i.name == model.name);
                if (item != null)
                    return BadRequest("Quyền đã tồn tại !");
                item = new Data.Entities.Role()
                {
                    name = model.name,
                    CreatedAt = DateTime.Now,
                    CreatedBy = model.CreatedBy,
                };
                db.Roles.Add(item);
                db.SaveChanges();
                return Ok(new {item});
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpPost]
        [Route("{id?}/staff")]
        public async Task<IActionResult> AddStaff(int? id, [FromBody] AddStaffViewModel model)
        {
            try
            {
                var role = db.Roles.FirstOrDefault(x => x.id == id);
                var staff = db.Staffs.FirstOrDefault(x => x.Id == model.id);
                if (role != null && staff != null)
                {
                    var staff_role = new StaffHasRole();
                    staff_role.Staff = staff;
                    staff_role.Role = role;
                    db.Add(staff_role);
                    await db.SaveChangesAsync();
                }
                return Ok(new { item = new { staff.Email, staff.Username, staff.Id, staff.Name } });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleViewModel model)
        {
            try
            {
                var item = db.Roles.FirstOrDefault(i => i.id == model.id);
                if (item != null)
                {
                    item.name = model.name;
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = model.UpdatedBy;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Delete(int id, int UpdatedBy)
        {
            try
            {
                var item = db.Roles.Include(x => x.StaffHasRoles).FirstOrDefault(i => i.id == id);
                if (item != null)
                {
                    item.DeletedAt = DateTime.Now;
                    item.UpdatedBy = UpdatedBy;
                    //db.Roles.Remove(item);
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var roleToDelete = db.Roles.Find(id);
            if (roleToDelete == null)
                return NotFound();
            db.Roles.Remove(roleToDelete);
            db.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id?}/staff/{staffid?}")]
        public async Task<IActionResult> DeleteUserInRole(int id, int staffId)
        {
            try
            {
                var item = db.Roles.Include(x => x.StaffHasRoles).FirstOrDefault(i => i.id == id);
                foreach (var staffHasRole in item.StaffHasRoles)
                {
                    if (staffHasRole.staffId == staffId)
                        db.Entry(staffHasRole).State = EntityState.Deleted;
                }
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
          * Quản lý permission trong role
          * - Hàm get permission
          * - Hàm cập nhật permission
          */
        [HttpGet]
        [Route("{id?}/permission")]
        public async Task<IActionResult> GetPermissionByRole(int? id)
        {
            try
            {
                //Get permission trong role
                var rolePermissions = await db.RoleHasPermissions.Where(x => x.roleId == id).ToListAsync();
                //Get tất cả permission
                var items = await db.Permissions.Select(x => new RolePermissionViewModel() { Id = x.id, Name = x.name, HasPermission = false, PermissionGroupId = x.PermissionGroupId })
                                                .OrderBy(x => x.Name).ToListAsync();
                //Bật giá trị true nếu permission đó có trong role
                foreach (var item in items)
                {
                    if (rolePermissions.FirstOrDefault(x => x.PermissionId == item.Id) != null)
                    {
                        item.HasPermission = true;
                    }
                }
                return Ok(new { items });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id?}/permission")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionViewModel model)
        {
            try
            {
                if (model.hasPermission)    //Thêm permission
                {
                    var rolePermission = db.RoleHasPermissions.Where(x => x.roleId == id && x.PermissionId == model.id).FirstOrDefault();
                    if (rolePermission == null)
                    {
                        rolePermission = new RoleHasPermission()
                        {
                            PermissionId = model.id,
                            roleId = id
                        };
                        db.RoleHasPermissions.Add(rolePermission);
                        await db.SaveChangesAsync();
                    }
                }
                else //Xóa permission
                {
                    var rolePermission = db.RoleHasPermissions.Where(x => x.roleId == id && x.PermissionId == model.id).FirstOrDefault();
                    if (rolePermission != null)
                    {
                        db.RoleHasPermissions.Remove(rolePermission);
                        await db.SaveChangesAsync();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
