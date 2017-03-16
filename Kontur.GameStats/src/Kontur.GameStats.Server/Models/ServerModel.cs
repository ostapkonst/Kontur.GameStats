using System.Collections.Generic;

namespace Kontur.GameStats.Server.Models
{
    public class ServerModel
    {
        public int Id { get; set; }
        public string endpoint { get; set; } // Получаем через URL
        public string name { get; set; }
        public List<GameModeModel> gameModes { get; set; }
        public List<MatcheModel> matches { get; set; }
    }

}
