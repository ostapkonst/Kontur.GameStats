using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kontur.GameStats.Server.Models.DTO
{
    [Matche]
    public class MatcheDTO
    {
        public string map { get; set; }
        public string gameMode { get; set; }
        public uint fragLimit { get; set; }      // Если поля не указаны
        public uint timeLimit { get; set; }      // в теле запроса, то
        public double timeElapsed { get; set; }  // будут нулевыми
        public List<ScoreBoardDTO> scoreboard { get; set; }
    }

    public class MatcheAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var matche = value as MatcheDTO;

            return matche.map?.Trim().Length > 0
                && matche.gameMode?.Trim().Length > 0
                && matche.fragLimit > 0
                && matche.timeLimit > 0
                && matche.timeElapsed <= matche.timeLimit
                && matche.scoreboard?.Count > 0
                && matche.scoreboard.All(x => x.frags <= matche.fragLimit)
                && matche.scoreboard.GroupBy(x => x.name).Count()
                == matche.scoreboard.Count
                // Т. к. OrderBy - не стабильная сортировка https://msdn.microsoft.com/en-us/library/dd383824.aspx
                && matche.scoreboard
                    .Select((pair, index) => new { pair, index })
                    .OrderByDescending(p => p.pair.frags)
                    .ThenBy(p => p.index)
                    .Select(p => p.pair)
                    .SequenceEqual(matche.scoreboard)
                && (matche.timeElapsed == matche.timeLimit
                    || matche.fragLimit == matche.scoreboard.Max(x => x.frags));
        }
    }
}