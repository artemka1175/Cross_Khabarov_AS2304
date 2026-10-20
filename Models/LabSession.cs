using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Khabarov_Artem_AS2304.Models
{
    public class LabSession // Ход проведения эксперимента конкретным пользователем и сохранение результатов.
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; } = string.Empty; // ФИО студента

        [Required]
        public string ExperimentTitle { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? FinishedAt { get; set; }

        public double FinalScore { get; set; } = 0.0; // Итоговый балл за опыт

        public bool IsPassed { get; set; } = false; // Зачет / Незачет
        public ICollection<LabSteps> Steps { get; set; } = new List<LabSteps>();         // Коллекция шагов, совершенных в рамках этого сеанса

    }
}