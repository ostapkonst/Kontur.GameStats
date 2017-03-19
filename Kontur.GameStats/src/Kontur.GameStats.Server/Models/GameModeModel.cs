namespace Kontur.GameStats.Server.Models
{
    public class GameModeModel
    {
        public int Id { get; set; }
        public string value { get; set; }

        public int ServerModelId { get; set; }
        public ServerModel ServerModel { get; set; }
    }
}