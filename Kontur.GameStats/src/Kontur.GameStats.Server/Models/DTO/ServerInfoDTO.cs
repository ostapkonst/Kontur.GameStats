using System.ComponentModel.DataAnnotations;

namespace Kontur.GameStats.Server.Models.DTO
{
    public class ServerInfoDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string[] gameModes { get; set; }
    }
}