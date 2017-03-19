using System.Collections.Generic;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Controllers;
using Kontur.GameStats.Server.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Kontur.GameStats.Server.Tests
{
    public class ServersTests
    {
        [Fact]
        public void InfoReturnEmptyListIfDatabaseIsEmpty()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("InfoEmpty")
                .Options;

            using (var context = new DatabaseContext(options))
            {
                var controller = new ServersController(context);
                var result = controller.Info();
                Assert.Empty(result);
            }
        }

        [Fact]
        public void InfoReturnListIfHaveOnlyAdvertise()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("InfoAdvertise")
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
                var controller = new ServersController(context);
                var result = controller.Info();
                Assert.NotEmpty(result);
            }
        }
    }
}