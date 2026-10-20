using Khabarov_Artem_AS2304.Data;
using Khabarov_Artem_AS2304.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Khabarov_Artem_AS2304.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly LabContext _context;

        public StepsController(LabContext context)
        {
            _context = context;
        }

        // Зарегистрировать шаг в рамках конкретной сессии
        [Authorize]
        [HttpPost("session/{sessionId}/perform")]
        public IActionResult PerformStep(int sessionId, [FromBody] LabSteps incomingStep)
        {
            var session = _context.LabSessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null)
            {
                return NotFound("Сессия лабораторной работы не найдена. Сначала создайте сессию через /api/sessions/start");
            }

            incomingStep.LabSessionId = session.Id;
            incomingStep.RecordedAt = DateTime.Now;

            _context.LabSteps.Add(incomingStep);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Шаг успешно зарегистрирован в базе данных SQLite!",
                SavedStep = incomingStep
            });
        }
    }
}