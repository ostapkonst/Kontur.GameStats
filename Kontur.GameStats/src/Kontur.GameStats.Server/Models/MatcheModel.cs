using System;
using System.Collections.Generic;

namespace Kontur.GameStats.Server.Models
{
    public class MatcheModel
    {
        public int Id { get; set; }
        public DateTime timestamp { get; set; } // Получаем через URL
        public string map { get; set; }
        public string gameMode { get; set; }
        public int fragLimit { get; set; }
        public int timeLimit { get; set; }
        public double timeElapsed { get; set; }
        public List<ScoreBoardModel> scoreboard { get; set; }

        public int ServerModelId { get; set; }
        public ServerModel ServerModel { get; set; }
    }
}
