namespace Kontur.GameStats.Server.Models
{
    public class ScoreBoardModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int frags { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }

        public int MatcheModelId { get; set; }
        public MatcheModel MatcheModel { get; set; }
    }
}
