using Khabarov_Artem_AS2304.Data;
using Khabarov_Artem_AS2304.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Khabarov_Artem_AS2304.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly LabContext _context;

        public SessionsController(LabContext context)
        {
            _context = context;
        }

        // Получить список всех сессий со всеми шагами
        [HttpGet]
        public ActionResult<IEnumerable<LabSession>> GetSessions()
        {
            return _context.LabSessions.Include(s => s.Steps).ToList();
        }

        // Создать новую сессию лабораторной работы для студента
        [Authorize]
        [HttpPost("start")]
        public IActionResult StartSession([FromQuery] string studentName, [FromQuery] string experimentTitle)
        {
            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(experimentTitle))
            {
                return BadRequest("Необходимо указать ФИО студента (studentName) и название эксперимента (experimentTitle).");
            }

            var session = new LabSession
            {
                StudentName = studentName,
                ExperimentTitle = experimentTitle,
                StartedAt = DateTime.Now,
                IsPassed = false,
                FinalScore = 0.0
            };

            _context.LabSessions.Add(session);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Сессия лабораторной работы успешно создана!",
                SessionId = session.Id,
                Session = session
            });
        }

        // Завершить лабораторную работу
        [Authorize]
        [HttpPost("{sessionId}/finish")]
        public IActionResult FinishSession(int sessionId)
        {
            var session = _context.LabSessions.Include(s => s.Steps).FirstOrDefault(s => s.Id == sessionId);
            if (session == null)
            {
                return NotFound("Сессия лабораторной работы не найдена.");
            }

            if (session.Steps == null || session.Steps.Count == 0)
            {
                return BadRequest("Нельзя завершить сессию, в которой не выполнено ни одного шага.");
            }

            double totalScore = 100.0;
            double penaltyPerError = 30.0; // Штраф за каждый неуспешный шаг

            foreach (var step in session.Steps)
            {
                if (!step.IsSuccess)
                {
                    totalScore -= penaltyPerError;
                }
            }

            // Оценка не может быть меньше 0
            if (totalScore < 0) totalScore = 0;

            session.FinishedAt = DateTime.Now;
            // Зачет ставится, если итоговый балл >= 50
            session.FinalScore = totalScore;
            session.IsPassed = totalScore >= 50.0;

            _context.SaveChanges();

            return Ok(new
            {
                Message = session.IsPassed
                    ? "Лабораторная работа успешно завершена!"
                    : "Лабораторная работа провалена из-за ошибок в ходе эксперимента.",
                Session = session
            });

        }
        [HttpGet("analytics")]
        public IActionResult GetLabAnalytics()
        {
            // LINQ запрос, объединяющий сессии, шаги и фильтрующий успешные результаты
            var analytics = _context.LabSessions
                .Where(s => s.IsPassed)
                .Join(
                    _context.LabSteps,
                    session => session.Id,
                    step => step.LabSessionId,
                    (session, step) => new
                    {
                        Student = session.StudentName,
                        Experiment = session.ExperimentTitle,
                        Action = step.ActionCode,
                        Formula = step.ReactionFormula,
                        Score = session.FinalScore
                    }
                )
                .ToList();

            return Ok(analytics);
        }
    }
}