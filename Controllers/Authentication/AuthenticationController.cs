

using LichTruc.Data;
using LichTruc.Data.Entities;
using LichTruc.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace LichTruc.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private AppDbContext db = null;
        private readonly IConfiguration configuration;
        public AuthenticationController(AppDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }
        public class StaffLoginModel
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost("staff")]
        public IActionResult GetStaffToken([FromBody] StaffLoginModel model)
        {
            var errorMessage = "The Staffname or password is incorrect. Please try again";
            if (!ModelState.IsValid) return BadRequest(errorMessage);

            var staff = db.Staffs.FirstOrDefault(x => x.Username == model.username && x.DeletedAt == null);
            if (staff == null) return BadRequest(errorMessage);

            if (staff.status == 0) return BadRequest("Account has no access permission");

            var result = AuthUtils.checkPasswordHash(model.password, staff.salt, staff.password);

            if (!result)
                return BadRequest(errorMessage);

            var token = GenerateStaffToken(staff);

            return Ok(token);
        }
        [HttpPost("staff/refresh")]
        public async Task<IActionResult> RefresStaffhtoken([FromBody] RefreshTokenViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var staff = await db.Staffs.SingleOrDefaultAsync(x => x.RefreshToken == model.RefreshToken && x.DeletedAt == null);

            if (staff == null) return BadRequest();

            //var token = GenerateStaffToken(staff, model.DepartmentId);
            var token = GenerateStaffToken(staff);

            return Ok(token);
        }

        //private TokenViewModel GenerateStaffToken(Staff staff, int departmentId = 0)
        private TokenViewModel GenerateStaffToken(Data.Entities.Staff staff)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, staff.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, staff.Name),
                new Claim(ClaimTypes.Name, staff.Name),
                new Claim(ClaimTypes.Sid, staff.Id.ToString()),
            };

            var permissions = GetStaffPermissionsViaRole(staff);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMins"]));

            var token = new JwtSecurityToken(
              configuration["Jwt:Issuer"],
              configuration["Jwt:Audience"],
              claims,
              expires: expires,
              signingCredentials: creds
            );

            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            staff.RefreshToken = refreshToken;
            db.Update(staff);
            db.SaveChanges();

            return new TokenViewModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Permissions = permissions,
                AccountId = staff.Id,
                Username = staff.Username,
                Name = staff.Name,
            };
        }
        private List<string> GetStaffPermissionsViaRole(Data.Entities.Staff staff)
        {
            var roles = db.StaffHasRoles.Where(x => x.staffId == staff.Id).ToList();
            var permisions = new List<string>();
            foreach (var role in roles)
            {
                permisions.AddRange(db.RoleHasPermissions.Include(x => x.Permission).Where(x => x.roleId == role.roleId).Select(x => x.Permission.code).ToList());
            }
            return permisions.Distinct().ToList();
        }
    }
}
