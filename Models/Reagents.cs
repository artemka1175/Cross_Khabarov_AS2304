using System.ComponentModel.DataAnnotations;

namespace Khabarov_Artem_AS2304.Models
{
    public class Reagents // Хранение свойств исследуемых веществ, реактивов и образцов.
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // "Стеарат натрия (Мыло)", "Дистиллированная вода"
        public string ChemicalFormula { get; set; } = string.Empty; // "C17H35COONa"
        public string StateOfMatter { get; set; } = string.Empty; // "Solid", "Liquid"
        public int HazardLevel { get; set; } = 1; // Класс опасности

    }
}