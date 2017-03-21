namespace Kontur.GameStats.Server.Models
{
    public class ScoreBoardModel
    {
        public int Id { get; set; }

        public int place { get; set; }
        public string name { get; set; }
        public uint frags { get; set; }
        public uint kills { get; set; }
        public uint deaths { get; set; }

        public int MatcheModelId { get; set; }
        public MatcheModel MatcheModel { get; set; }
    }
}