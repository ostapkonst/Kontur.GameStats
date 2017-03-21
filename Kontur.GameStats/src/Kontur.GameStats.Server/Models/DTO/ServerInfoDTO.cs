﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kontur.GameStats.Server.Models.DTO
{
    [ServerInfo]
    public class ServerInfoDTO
    {
        public string name { get; set; }
        public string[] gameModes { get; set; }
    }

    public class ServerInfoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var info = value as ServerInfoDTO;

            return info.name?.Trim().Length > 0
                && info.gameModes?.Length > 0
                && info.gameModes.All(x => x.Trim().Length > 0);
        }
    }
}