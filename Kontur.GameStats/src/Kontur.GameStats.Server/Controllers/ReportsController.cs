using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Kontur.GameStats.Server.Context;
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

        // GET /reports/recent-matches[/<count>]
        [ActionName("recent-matches")]
        public IEnumerable<object> RecentMatches(int count = 5)
        {
            count = count < 1 ? 1 : count > 50 ? 50 : count;

            var query = db.Matches
                .Include(s => s.ServerModel)
                .Include(s => s.scoreboard)
                .OrderByDescending(s => s.timestamp);

            return
                query.Select(
                    x => new
                    {
                        server = x.ServerModel.endpoint,
                        timestamp = x.timestamp
                            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
                        results = new
                        {
                            map = x.map,
                            gameMode = x.gameMode,
                            fragLimit = x.fragLimit,
                            timeLimit = x.timeLimit,
                            timeElapsed = x.timeElapsed,
                            scoreboard = x.scoreboard.Select(
                                y => new
                                {
                                    name = y.name,
                                    frags = y.frags,
                                    kills = y.kills,
                                    deaths = y.deaths
                                }
                            )
                        }
                    }
                ).Take(count);
        }

        // GET /reports/best-players[/<count>]
        [ActionName("best-players")]
        public IEnumerable<object> BestPlayers(int count = 5)
        {
            count = count < 1 ? 1 : count > 50 ? 50 : count;

            var query = db.ScoreBoards
                .Include(s => s.MatcheModel)
                    .ThenInclude(s => s.ServerModel);

            return
                query.GroupBy(x => x.name)
                .Select(y => new
                {
                    name = y.First().name,
                    totalKills = y.Sum(x => x.kills),
                    totalDeaths = y.Sum(x => x.deaths)
                })
                .Where(x => x.totalKills > 0 && x.totalDeaths >= 10)
                .Select(x => new
                {
                    x.name,
                    killToDeathRatio = (float)x.totalKills / x.totalDeaths
                })
                .OrderByDescending(x => x.killToDeathRatio)
                .Take(count);
        }

        // GET /reports/popular-servers[/<count>]
        [ActionName("popular-servers")]
        public IEnumerable<object> PopularServers(int count = 5)
        {
            count = count < 1 ? 1 : count > 50 ? 50 : count;

            var query = db.Servers
                .Include(s => s.matches)
                    .ThenInclude(m => m.scoreboard);

            return
                query
                .Select(y => new
                {
                    endpoint = y.endpoint,
                    name = y.name,
                    averageMatchesPerDay =
                        y.matches.Any()
                        ?
                        y.matches.GroupBy(m => m.timestamp.Date)
                        .Average(g => (float)g.Count())
                        :
                        0
                })
                .OrderByDescending(x => x.averageMatchesPerDay)
                .Take(count);
        }
    }
}
