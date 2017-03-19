using System;
using System.Collections.Generic;
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

            if (query == null)
                return NotFound();

            var MatchesPerDay = query.matches
                .GroupBy(x => x.timestamp.Date)
                .Select(x => x.Count());

            // Если на сервере еще не было сыграно ни одного матча
            if (!MatchesPerDay.Any())
                MatchesPerDay = new List<int> { 0 };

            var Population = query.matches
                .Select(x => x.scoreboard.Count);

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

            if (!query.Any())
                return NotFound();

            var MatchesPerDay = query
                .GroupBy(x => x.MatcheModel.timestamp.Date)
                .Select(x => x.Count());

            var player = new
            {
                totalMatchesPlayed = query.Count(),
                totalMatchesWon = query
                    .Count(x => x.frags == x.MatcheModel.scoreboard.Max(y => y.frags)),
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
                    .Select(x => (double)x.playersBelowCurrent / (x.totalPlayers - 1) * 100)
                    .Average(x => double.IsNaN(x) ? 100 : x),
                maximumMatchesPerDay = MatchesPerDay.Max(),
                averageMatchesPerDay = MatchesPerDay.Average(),
                lastMatchPlayed = query
                    .OrderByDescending(x => x.MatcheModel.timestamp)
                    .Select(x => x.MatcheModel.timestamp)
                    .First()
                    .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
                killToDeathRatio = 
                    ((float)query.Sum(x => x.kills) / query.Sum(x => x.deaths)).HasValue(0)
            };

            return Ok(player);
        }
    }
}