using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Kontur.GameStats.Server.Context;
using Microsoft.EntityFrameworkCore;


namespace Kontur.GameStats.Server.Controllers
{
    public class StatsController : Controller
    {
        private readonly DatabaseContext db;

        public StatsController(DatabaseContext context)
        {
            db = context;
        }

        // GET /servers/<endpoint>/stats
        [HttpGet("[action]/{endpoint}/[controller]")]
        public IActionResult Servers(string endpoint)
        {
            var query = db.Servers
                .Where(s => s.endpoint == endpoint)
                .Include(s => s.matches)
                    .ThenInclude(m => m.scoreboard)
                .FirstOrDefault();

            if (query == null)
                return NotFound();

            var MatchesPerDay = query.matches
                .GroupBy(m => m.timestamp.Date)
                .Select(g => g.Count());

            // Если на сервере еще не было сыграно ни одного матча
            if (!MatchesPerDay.Any())
                MatchesPerDay = new List<int> { 0 };

            var Population = query.matches
                .Select(s => s.scoreboard.Count);

            // Если игровой сервер вернул матч без списка игроков
            if (!Population.Any())
                Population = new List<int> { 0 };

            var server = new
            {
                totalMatchesPlayed = query.matches.Count,

                maximumMatchesPerDay = MatchesPerDay.Max(),

                averageMatchesPerDay = MatchesPerDay.Average(),

                maximumPopulation = Population.Max(),

                averagePopulation = Population.Average(),

                top5GameModes = query.matches
                    .GroupBy(m => m.gameMode)
                    .OrderByDescending(m => m.Count())
                    .Select(g => g.Key)
                    .Take(5),

                top5Maps = query.matches
                    .GroupBy(m => m.map)
                    .OrderByDescending(m => m.Count())
                    .Select(g => g.Key)
                    .Take(5)
            };

            return Ok(server);
        }

        // ASP.NET Core is URLDecoding automatically
        // GET /players/<name>/stats
        [HttpGet("[action]/{name}/[controller]")]
        public IActionResult Players(string name)
        {
            var query = db.ScoreBoards
                .Where(s => s.name == name)
                .Include(s => s.MatcheModel)
                    .ThenInclude(s => s.ServerModel);

            if (!query.Any())
                return NotFound();

            var MatchesPerDay = query
                .GroupBy(s => s.MatcheModel.timestamp.Date)
                .Select(s => s.Count());

            var player = new
            {
                totalMatchesPlayed = query.Count(),

                totalMatchesWon = query
                    .Where(s => s.frags == s.MatcheModel.scoreboard.Max(p => p.frags))
                    .Count(),

                favoriteServer = query
                    .GroupBy(s => s.MatcheModel.ServerModel.endpoint)
                    .OrderByDescending(s => s.Count())
                    .Select(s => s.Key)
                    .First(),

                uniqueServers = query
                    .GroupBy(s => s.MatcheModel.ServerModel.endpoint)
                    .Count(),

                favoriteGameMode = query
                    .GroupBy(s => s.MatcheModel.gameMode)
                    .OrderByDescending(s => s.Count())
                    .Select(s => s.Key)
                    .First(),

                averageScoreboardPercent = query
                    .Select(
                        x => new
                        {
                            totalPlayers =
                            x.MatcheModel.scoreboard.Count,

                            playerStats =
                            x.MatcheModel.scoreboard.OrderByDescending(y => y.frags)
                            .ToList()
                            .IndexOf(x)
                        }).ToList()
                    .Select(
                        x => new
                        {
                            x.totalPlayers,
                            playersBelowCurrent = x.totalPlayers - 1 - x.playerStats
                        })
                    // ВАЖНО: Формула изменена в соответствии с логикой использования
                    .Select(x => (double)x.playersBelowCurrent / (x.totalPlayers - 1) * 100)
                    .Average(x => double.IsNaN(x) ? 0 : x),

                maximumMatchesPerDay = MatchesPerDay.Max(),

                averageMatchesPerDay = MatchesPerDay.Average(),

                lastMatchPlayed = query
                    .OrderByDescending(s => s.MatcheModel.timestamp)
                    .Select(s => s.MatcheModel.timestamp)
                    .First()
                    .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),

                // Возвращает Infinity или NaN при делении на 0, требуется уточнение...
                killToDeathRatio = ((float)query
                    .Sum(s => s.kills) / query.Sum(s => s.deaths))
            };

            return Ok(player);
        }
    }
}
