using Khabarov_Artem_AS2304.Data;
using Khabarov_Artem_AS2304.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Khabarov_Artem_AS2304.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private readonly LabContext _context;

        public LabController(LabContext context)
        {
            _context = context;
        }

        // ѕолучить состо€ние лабораторного оборудовани€ и установок
        [HttpGet("apparatuses")]
        public async Task<ActionResult<IEnumerable<LabApparatus>>> GetApparatuses()
        {
            return await _context.LabApparatuses
                .Include(a => a.LoadedReagent)
                .ToListAsync();
        }

        // ƒобавление новой установки/прибора (ƒоступно только Admin)
        [HttpPost("apparatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddApparatus([FromBody] LabApparatus apparatus)
        {
            if (apparatus == null)
            {
                return BadRequest("ƒанные установки не переданы.");
            }

            _context.LabApparatuses.Add(apparatus);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Ћабораторна€ установка успешно добавлена!",
                Data = apparatus
            });
        }
    }
}