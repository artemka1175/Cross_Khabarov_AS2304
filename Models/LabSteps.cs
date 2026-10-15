using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khabarov_Artem_AS2304.Models
{
    public class LabSteps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LabSessionId { get; set; }

        [ForeignKey("LabSessionId")]
        public LabSession? LabSession { get; set; }

        [Required]
        public string ActionDescription { get; set; } = string.Empty; // Что сделал: "Добавил 20г мыла и 3 мл воды в пробирку"

        public DateTime RecordedAt { get; set; } = DateTime.Now;

        public string ResultOutput { get; set; } = string.Empty; // Итог / Наблюдение: "Мыло растворилось при нагреве. pH = 9 (щелочная среда)"

        public bool IsSuccess { get; set; } = true; // Успех действия (false, если нарушена последовательность или допущена ошибка)
    }
}
