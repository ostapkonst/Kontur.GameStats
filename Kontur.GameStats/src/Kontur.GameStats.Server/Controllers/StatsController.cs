using System.Linq;
using Kontur.GameStats.Server.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kontur.GameStats.Server.Controllers
{
    [Route("[action]")]
    public class StatsController : Controller
    {
        private readonly DatabaseContext db;

        public StatsController(DatabaseContext context)
        {
            db = context;
        }

        // GET: /servers/<endpoint>/stats
        [HttpGet("{endpoint:endpoint}/[controller]")]
        public IActionResult Servers(string endpoint)
        {
            var query = db.Servers
                .Where(x => x.endpoint == endpoint)
                .Include(x => x.matches)
                .ThenInclude(x => x.scoreboard)
                .FirstOrDefault();

            if (query == null) return NotFound();

            var matchesPerDay = query.matches
                .GroupBy(x => x.timestamp.Date)
                .Select(x => x.Count())
                .DefaultIfEmpty();

            var population = query.matches
                .Select(x => x.scoreboard.Count)
                .DefaultIfEmpty();

            var server = new
            {
                totalMatchesPlayed = query.matches.Count,
                maximumMatchesPerDay = matchesPerDay.Max(),
                averageMatchesPerDay = matchesPerDay.Average(),
                maximumPopulation = population.Max(),
                averagePopulation = population.Average(),
                top5GameModes = query.matches
                    .GroupBy(x => x.gameMode)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .Take(5),
                top5Maps = query.matches
                    .GroupBy(x => x.map)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .Take(5)
            };

            return Ok(server);
        }

        // GET: /players/<name>/stats
        [HttpGet("{name}/[controller]")]
        public IActionResult Players(string name)
        {
            var query = db.ScoreBoards
                .Where(x => x.name == name)
                .Include(x => x.MatcheModel)
                .ThenInclude(x => x.ServerModel);

            if (!query.Any()) return NotFound();

            var matchesPerDay = query
                .GroupBy(x => x.MatcheModel.timestamp.Date)
                .Select(x => x.Count());

            var player = new
            {
                totalMatchesPlayed = query.Count(),
                totalMatchesWon = query.Count(x => x.place == 0),
                favoriteServer = query
                    .GroupBy(x => x.MatcheModel.ServerModel.endpoint)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .First(),
                uniqueServers = query
                    .GroupBy(x => x.MatcheModel.ServerModel.endpoint)
                    .Count(),
                favoriteGameMode = query
                    .GroupBy(x => x.MatcheModel.gameMode)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .First(),
                averageScoreboardPercent = query
                    .Select(
                        x => new
                        {
                            totalPlayers = x.MatcheModel.scoreboard.Count,
                            playerStats = x.place
                        }).ToList()
                    .Select(
                        x => new
                        {
                            x.totalPlayers,
                            playersBelowCurrent = x.totalPlayers - 1 - x.playerStats
                        })
                    .Select(x => x.playersBelowCurrent * 100d / (x.totalPlayers - 1))
                    .Average(x => x.HasValue(100)),
                maximumMatchesPerDay = matchesPerDay.Max(),
                averageMatchesPerDay = matchesPerDay.Average(),
                lastMatchPlayed = query
                    .OrderByDescending(x => x.MatcheModel.timestamp)
                    .Select(x => x.MatcheModel.timestamp)
                    .First()
                    .ToUtcZ(),
                killToDeathRatio =
                    ((double)query.Sum(x => x.kills) / query.Sum(x => x.deaths)).HasValue(0)
            };

            return Ok(player);
        }
    }
}