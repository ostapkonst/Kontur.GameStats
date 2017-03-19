using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kontur.GameStats.Server.Models.DTO
{
    public class MatcheDTO
    {
        [Required]
        public string map { get; set; }
        [Required]
        public string gameMode { get; set; }
        public int fragLimit { get; set; }      // Если поля не указаны
        public int timeLimit { get; set; }      // в теле запроса, то
        public double timeElapsed { get; set; } // будут нулевыми
        [Required]
        public List<ScoreBoardDTO> scoreboard { get; set; }
    }
}