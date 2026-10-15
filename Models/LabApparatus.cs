using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khabarov_Artem_AS2304.Models
{
    public class LabApparatus // Моделирование состояния приборов и установок, используемых в эксперименте.
    
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ApparatusType { get; set; } = "Пробирка с горелкой"; // Тип оборудования
        public bool IsHeaterOn { get; set; } = false; // Нагрев включен/выключен
        public double CurrentTemperatureC { get; set; } = 20.0; // Температура содержимого 
        public double VolumeMl { get; set; } = 0.0; // Общий объем смеси в пробирке
        public int? LoadedReagentId { get; set; }

        [ForeignKey("LoadedReagentId")]
        public Reagents? LoadedReagent { get; set; }
        public void ToggleHeater(bool state) // Метод моделирования нагрева (по методичке: при нагревании мыло растворяется быстрее)
        {
            IsHeaterOn = state;
            if (IsHeaterOn)
            {
                CurrentTemperatureC += 35.0; // Имитация нагрева пламенем горелки
            }
        }
    }
}