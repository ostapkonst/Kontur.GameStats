using Microsoft.EntityFrameworkCore;
using Kontur.GameStats.Server.Models;

namespace Kontur.GameStats.Server.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ServerModel> Servers { get; set; }
        public DbSet<GameModeModel> GameModes { get; set; }
        public DbSet<MatcheModel> Matches { get; set; }
        public DbSet<ScoreBoardModel> ScoreBoards { get; set; }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
            // Производим миграцию, если базу данных удалили
            // Database.Migrate();
        }
    }
}