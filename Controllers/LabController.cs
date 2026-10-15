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
        [HttpGet("reagents")]
        public ActionResult<IEnumerable<Reagents>> GetReagents()
        {
            return _context.Reagents.ToList();
        }

        // Получаем состояние лабораторного оборудования (доступно всем)
        [HttpGet("apparatuses")]
        public ActionResult<IEnumerable<LabApparatus>> GetApparatuses()
        {
            return _context.LabApparatuses.Include(a => a.LoadedReagent).ToList();
        }

        // 3. Симуляция шага ( нагрев пробирки и растворение мыла )
        [Authorize]
        [HttpPost("sessions/{sessionId}/perform-step")]
        public IActionResult PerformSoapExperimentStep(int sessionId, [FromQuery] bool turnOnHeater)
        {
            var session = _context.LabSessions.Include(s => s.Steps).FirstOrDefault(s => s.Id == sessionId);
            if (session == null)
            {
                // Если сессия не найдена, создаем новую для примера
                session = new LabSession { StudentName = "Артём Хабаров", ExperimentTitle = "Опыт 1. Растворение мыла в воде" };
                _context.LabSessions.Add(session);
                _context.SaveChanges();
            }

            var apparatus = _context.LabApparatuses.FirstOrDefault();
            if (apparatus == null) return NotFound("Установка не найдена.");

            // Моделируем логику опыта по методичке
            apparatus.ToggleHeater(turnOnHeater);

            string actionDesc = turnOnHeater ? "Включен нагрев горелки" : "Выключен нагрев горелки";
            string resultText = turnOnHeater
                ? $"Температура повышена до {apparatus.CurrentTemperatureC}°C. Мыло растворяется быстрее. Среда щелочная (pH > 7)."
                : "Нагрев прекращен. Температура стабильна.";

            // Записываем шаг в историю (модуль LabStep)
            var step = new LabSteps
            {
                LabSessionId = session.Id,
                ActionDescription = actionDesc,
                ResultOutput = resultText,
                IsSuccess = true
            };

            _context.LabSteps.Add(step);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Шаг эксперимента успешно выполнен!",
                ApparatusState = apparatus,
                RecordedStep = step
            });
        }
    }
}