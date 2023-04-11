using LichTruc.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LichTruc.Interfaces
{
        public interface IAreaController
        {
            Task<IActionResult> GetAllAreas();
            Task<IActionResult> GetArea(int id);
            Task<IActionResult> CreateArea(Area model);
            Task<IActionResult> UpdateArea(int id, Area model);
            Task<IActionResult> DeleteArea(int id);
        }
}
