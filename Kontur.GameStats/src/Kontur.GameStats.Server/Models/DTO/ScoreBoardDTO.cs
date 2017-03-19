using System.ComponentModel.DataAnnotations;

namespace Kontur.GameStats.Server.Models.DTO
{
    public class ScoreBoardDTO
    {
        [Required]
        public string name { get; set; }
        public int frags { get; set; }  // Если поля не указаны
        public int kills { get; set; }  // в теле запроса, то
        public int deaths { get; set; } // будут нулевыми
    }
}