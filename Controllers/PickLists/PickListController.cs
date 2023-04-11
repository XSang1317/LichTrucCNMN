using LichTruc.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LichTruc.Data;

namespace LichTruc.Controllers.PickLists
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickListController : ControllerBase
    {
        private AppDbContext db = null;
        public PickListController(AppDbContext db)
        {
            this.db = db;
        }
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetbySlug(string slug, string q)
        {
            var items = db.PickLists
                          .Where(i => i.DeletedAt == null && i.PickListType == slug)
                          .OrderBy(x => x.SortOrder)
                          .Select(x => new { x.id, x.PickListValue, x.PickListType, x.SortOrder }).ToList();
            if (!string.IsNullOrEmpty(q))
            {
                var Query = $"%{q?.ToLower()}%";
                items = items.Where(x => string.IsNullOrEmpty(q)
                                                || (EF.Functions.Like(x.PickListValue.ToLower(), Query)))
                                                .OrderBy(x => x.SortOrder).ToList();
            }
            return Ok(items);
        }
        public async Task<IActionResult> Update([FromBody] UpdatePickList model)
        {
            try
            {
                var item = db.PickLists.Where(x => x.DeletedAt == null).FirstOrDefault(i => i.id == model.id);
                if (item != null)
                {
                    item.PickListValue = model.pickListValue;
                    item.UpdatedBy = model.UpdatedBy;
                    item.UpdatedAt = DateTime.Now;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] PickList model)
        {
            try
            {
                try
                {
                    model.SortOrder = db.PickLists.Where(x => x.PickListType.Equals(model.PickListType)).Max(x => x.SortOrder) + 1;
                }
                catch
                {
                    model.SortOrder = 0;
                }
                model.CreatedAt = DateTime.Now;
                db.PickLists.Add(model);
                db.SaveChanges();
                return Ok(new { model.id, model.PickListValue, model.PickListType, model.SortOrder });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Delete(int id, int UpdatedBy)
        {
            try
            {
                var item = db.PickLists.FirstOrDefault(i => i.id == id);
                if (item != null)
                {
                    item.DeletedAt = DateTime.Now;
                    item.UpdatedBy = UpdatedBy;
                    //item.PickListValue = item.PickListValue + "_x_" + DateTime.Now;
                    await db.SaveChangesAsync();
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
