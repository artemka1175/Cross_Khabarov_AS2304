using Khabarov_Artem_AS2304.Data;
using Khabarov_Artem_AS2304.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Khabarov_Artem_AS2304.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReagentsController : ControllerBase
    {
        private readonly LabContext _context;

        public ReagentsController(LabContext context)
        {
            _context = context;
        }

        // Получить список всех доступных реактивов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reagents>>> GetReagents()
        {
            return await _context.Reagents.ToListAsync();
        }


        // Добавление нового реактива/вещества (Доступно только Admin)
 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddReagent([FromBody] Reagents reagent)
        {
            if (reagent == null)
            {
                return BadRequest("Данные реактива не переданы.");
            }

            _context.Reagents.Add(reagent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Новый реактив успешно добавлен!",
                Data = reagent
            });
        }
    }
}