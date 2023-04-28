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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data.Entities.ShiftsType>>> GetShiftTypes()
        {
            return await db.ShiftsTypes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Data.Entities.ShiftsType>> GetShiftType(int id)
        {
            var shiftType = await db.ShiftsTypes.FindAsync(id);

            if (shiftType == null)
            {
                return NotFound();
            }

            return shiftType;
        }

        [HttpPost]
        public async Task<ActionResult<ShiftsType>> PostShiftType(ShiftsType shiftType)
        {
            db.ShiftsTypes.Add(shiftType);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShiftType), new { id = shiftType.id }, shiftType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShiftType(int id, Data.Entities.ShiftsType shiftType)
        {
            if (id != shiftType.id)
            {
                return BadRequest();
            }

            db.Entry(shiftType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftTypeExists(id))
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
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteShiftType(int id)
            {
                var shiftType = await db.ShiftsTypes.FindAsync(id);
                if (shiftType == null)
                {
                    return NotFound();
                }

                db.ShiftsTypes.Remove(shiftType);
                await db.SaveChangesAsync();

                return NoContent();
            }
            private bool ShiftTypeExists(int id)
        {
            return db.ShiftsTypes.Any(e => e.id == id);
        }
    }
}
