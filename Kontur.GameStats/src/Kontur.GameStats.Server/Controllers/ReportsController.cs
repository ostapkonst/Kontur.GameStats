using System.Collections.Generic;
using System.Linq;
using Kontur.GameStats.Server.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kontur.GameStats.Server.Controllers
{
    [Route("[controller]/[action]/{count:int?}")]
    public class ReportsController : Controller
    {
        private readonly DatabaseContext db;

        public ReportsController(DatabaseContext context)
        {
            db = context;
        }

        // GET: /reports/recent-matches[/<count>]
        [ActionName("recent-matches")]
        public IEnumerable<object> RecentMatches(int count = 5)
        {
            var query = db.Matches
                .Include(x => x.ServerModel)
                .Include(x => x.scoreboard)
                .OrderByDescending(x => x.timestamp);

            return query
                .Select(
                    x => new
                    {
                        server = x.ServerModel.endpoint,
                        timestamp = x.timestamp
                            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
                        results = new
                        {
                            x.map,
                            x.gameMode,
                            x.fragLimit,
                            x.timeLimit,
                            x.timeElapsed,
                            scoreboard = x.scoreboard
                                .Select(
                                    y => new
                                    {
                                        y.name,
                                        y.frags,
                                        y.kills,
                                        y.deaths
                                    }
                            )
                        }
                    }
            ).Take(count < 50 ? count : 50);
        }

        // GET: /reports/best-players[/<count>]
        [ActionName("best-players")]
        public IEnumerable<object> BestPlayers(int count = 5)
        {
            var query = db.ScoreBoards
                .Include(x => x.MatcheModel)
                .ThenInclude(x => x.ServerModel);

            return query
                .GroupBy(x => x.name)
                .Select(
                    x => new
                    {
                        x.First().name,
                        playedMatches = x.Count(),
                        totalKills = x.Sum(y => y.kills),
                        totalDeaths = x.Sum(y => y.deaths)
                    })
                .Where(x => x.playedMatches >= 10 && x.totalDeaths >= 1)
                .Select(
                    x => new
                    {
                        x.name,
                        killToDeathRatio = (float)x.totalKills / x.totalDeaths
                    })
                .OrderByDescending(x => x.killToDeathRatio)
                .Take(count < 50 ? count : 50);
        }

        // GET: /reports/popular-servers[/<count>]
        [ActionName("popular-servers")]
        public IEnumerable<object> PopularServers(int count = 5)
        {
            var query = db.Servers
                .Include(x => x.matches)
                .ThenInclude(x => x.scoreboard);

            return query
                .Select(
                    x => new
                    {
                        endpoint = x.endpoint,
                        name = x.name,
                        averageMatchesPerDay =
                            x.matches.Any()
                                ? x.matches.GroupBy(y => y.timestamp.Date)
                                    .Average(y => (float)y.Count())
                                : 0
                    })
                .OrderByDescending(x => x.averageMatchesPerDay)
                .Take(count < 50 ? count : 50);
        }
    }
}