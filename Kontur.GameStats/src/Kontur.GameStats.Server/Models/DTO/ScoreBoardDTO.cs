using System.ComponentModel.DataAnnotations;

namespace Kontur.GameStats.Server.Models.DTO
{
    // При необходимости атрибут валидации можно отключить
    [ScoreBoard]
    public class ScoreBoardDTO
    {
        public string name { get; set; }
        public uint frags { get; set; }  // Если поля не указаны
        public uint kills { get; set; }  // в теле запроса, то
        public uint deaths { get; set; } // будут нулевыми
    }

    public class ScoreBoardAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var score = value as ScoreBoardDTO;

            return score.name?.Trim().Length > 0;
        }
    }
}