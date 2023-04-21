using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;
using LichTruc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Controllers.Staff
{
    [Route("api/[controller]")]
    public class Staffs1Controller : ControllerBase
    {
        private readonly AppDbContext _context;
        public Staffs1Controller(AppDbContext context)
        {
            _context = context;
        }
        //GetALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichTruc.Data.Entities.Staff>>> GetStaffs()
        {
            return await _context.Staffs.ToListAsync();
        }
        //Get Staffs {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LichTruc.Data.Entities.Staff>> GetStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }
        //Create
        [HttpPost]
        public async Task<ActionResult<LichTruc.Data.Entities.Staff>> PostStaff(LichTruc.Data.Entities.Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStaff), new { id = staff.Id }, staff);
        }
        //Edit Staffs
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, LichTruc.Data.Entities.Staff staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //Delete Staffs
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

