using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Models;
using Kontur.GameStats.Server.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kontur.GameStats.Server.Controllers
{
    [Route("[controller]")]
    public class ServersController : Controller
    {
        private readonly DatabaseContext db;

        public ServersController(DatabaseContext context)
        {
            db = context;
        }

        // GET: /servers/info
        [HttpGet("[action]")]
        public IEnumerable<object> Info()
        {
            return db.Servers
                .Include(x => x.gameModes)
                .Select(
                    x => new
                    {
                        x.endpoint,
                        info = new ServerInfoDTO
                        {
                            name = x.name,
                            gameModes = x.gameModes.Select(y => y.value).ToArray()
                        }
                    });
        }

        // GET: /servers/<endpoint>/info
        [HttpGet("{endpoint:endpoint}/[action]")]
        public IActionResult Info(string endpoint)
        {
            var query = db.Servers
                .Where(x => x.endpoint == endpoint)
                .Include(x => x.gameModes)
                .FirstOrDefault();

            if (query == null) return NotFound();

            var info = new ServerInfoDTO
            {
                name = query.name,
                gameModes = query.gameModes.Select(x => x.value).ToArray()
            };

            return Ok(info);
        }

        // PUT: /servers/<endpoint>/info
        [HttpPut("{endpoint:endpoint}/[action]")]
        public IActionResult Info(string endpoint, [FromBody] ServerInfoDTO server)
        {
            if (server == null || !ModelState.IsValid)
                return BadRequest();

            var query = db.Servers
                .Where(x => x.endpoint == endpoint)
                .Include(x => x.gameModes)
                .FirstOrDefault();

            var model = new ServerModel
            {
                endpoint = endpoint,
                name = server.name,
                gameModes = (server.gameModes
                    .Select(
                        x => new GameModeModel
                        {
                            value = x
                        }).ToList())
            };

            if (query == null)
                db.Servers.Add(model);
            else
            {
                query.name = model.name;
                query.gameModes = model.gameModes;
            }

            db.SaveChanges();
            return Ok();
        }

        // GET: /servers/<endpoint>/matches/<timestamp>
        [HttpGet("{endpoint:endpoint}/[action]/{timestamp:datetime}Z")]
        public IActionResult Matches(string endpoint, DateTime timestamp)
        {
            var query = db.Servers
                .Where(x => x.endpoint == endpoint)
                .Include(x => x.matches)
                .ThenInclude(x => x.scoreboard)
                .FirstOrDefault();

            if (query == null) return NotFound();

            var match = query.matches
                .Where(x => x.timestamp == timestamp)
                .Select(
                    x => new MatcheDTO
                    {
                        map = x.map,
                        gameMode = x.gameMode,
                        fragLimit = x.fragLimit,
                        timeLimit = x.timeLimit,
                        timeElapsed = x.timeElapsed,
                        scoreboard = x.scoreboard
                            .Select(
                                y => new ScoreBoardDTO
                                {
                                    name = y.name,
                                    frags = y.frags,
                                    kills = y.kills,
                                    deaths = y.deaths
                                }).ToList()
                    }
                );

            if (match.Any())
                return Ok(match.First());
            else
                return NotFound();
        }

        // PUT: /servers/<endpoint>/matches/<timestamp>
        [HttpPut("{endpoint:endpoint}/[action]/{timestamp:datetime}Z")]
        public IActionResult Matches(string endpoint, DateTime timestamp,
            [FromBody] MatcheDTO match)
        {
            if (match == null || !ModelState.IsValid)
                return BadRequest();

            var query = db.Servers
                .Where(x => x.endpoint == endpoint)
                .Include(x => x.gameModes)
                .Include(x => x.matches)
                .FirstOrDefault();

            if (query == null
                || query.matches.Count(x => x.timestamp == timestamp) > 0
                || query.gameModes // Проверка поддержки игрового режима
                    .Count(x => x.value == match.gameMode) == 0)
                return BadRequest();

            var model = new MatcheModel
            {
                timestamp = timestamp,
                map = match.map,
                gameMode = match.gameMode,
                fragLimit = match.fragLimit,
                timeLimit = match.timeLimit,
                timeElapsed = match.timeElapsed,
                scoreboard = match.scoreboard
                    .Select(
                        (x, y) => new ScoreBoardModel
                        {
                            place = y,
                            name = x.name,
                            frags = x.frags,
                            kills = x.kills,
                            deaths = x.deaths
                        }).ToList()
            };

            query.matches.Add(model);

            db.SaveChanges();
            return Ok();
        }
    }
}