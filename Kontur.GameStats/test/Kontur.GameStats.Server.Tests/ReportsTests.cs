using System.Collections.Generic;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Controllers;
using Kontur.GameStats.Server.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Kontur.GameStats.Server.Tests
{
    public class ReportsTests
    {
        [Fact]
        public void RecentMatchesReturnEmptyListIfDatabaseIsEmpty()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("RecentMatchesEmpty")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.RecentMatches();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void RecentMatchesReturnEmptyListIfHaveOnlyAdvertise()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("RecentMatchesAdvertise")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var server = new ServerModel
                {
                    endpoint = "167.42.23.32-1337",
                    name = "] My P3rfect Server [",
                    gameModes = new List<GameModeModel>
                    {
                        new GameModeModel {value = "DM"},
                        new GameModeModel {value = "TDM"}
                    }
                };

                context.Servers.Add(server);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.RecentMatches();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void BestPlayersReturnEmptyListIfDatabaseIsEmpty()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("BestPlayersEmpty")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.BestPlayers();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void BestPlayersReturnEmptyListIfHaveOnlyAdvertise()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("BestPlayersAdvertise")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var server = new ServerModel
                {
                    endpoint = "167.42.23.32-1337",
                    name = "] My P3rfect Server [",
                    gameModes = new List<GameModeModel>
                    {
                        new GameModeModel {value = "DM"},
                        new GameModeModel {value = "TDM"}
                    }
                };

                context.Servers.Add(server);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.BestPlayers();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void PopularServersReturnEmptyListIfDatabaseIsEmpty()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("PopularServersEmpty")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.PopularServers();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void PopularServersReturnListIfHaveOnlyAdvertise()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("PopularServersAdvertise")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var server = new ServerModel
                {
                    endpoint = "167.42.23.32-1337",
                    name = "] My P3rfect Server [",
                    gameModes = new List<GameModeModel>
                    {
                        new GameModeModel {value = "DM"},
                        new GameModeModel {value = "TDM"}
                    }
                };

                context.Servers.Add(server);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext(options))
            {
                var controller = new ReportsController(context);
                var result = controller.PopularServers();
                Assert.NotEmpty(result);
            }
        }
    }
}