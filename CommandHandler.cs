using System;
using System.Linq;
using System.Collections.Generic;
using Exiled.API.Features;

using _Server = Exiled.API.Features.Server;
using _Map = Exiled.API.Features.Map;
using _Player = Exiled.API.Features.Player;

namespace RemoteConnection
{
    public static class CommandHandler
    {
        public static DateTime startTime = DateTime.Now;
        public static string GetPlayerList()
        {
            List<Player> players = Player.List.ToList();

            string playerInformation = null;
            foreach (var player in players) 
            {
                playerInformation += $"{player.Nickname},{player.UserId},{player.Id},";
            }

            return playerInformation;
        }

        public static string GetUptime()
        {
            TimeSpan uptime = DateTime.Now.Subtract(startTime);
            return $"{Math.Round(uptime.TotalMinutes,2)} minutes";
        }

        public static string Broadcast(string[] message)
        {
            _Map.Broadcast(4, message[0]);
            return "Message Broadcasted.";
        }

        public static string Ban(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    _Server.BanPlayer.BanUser(_Player.Get(args[i]).GameObject, 2678400, "Unspecified", "Remote Connection");
                }
                return "User(s) banned.";
            }
            catch{ return "An error occured while banning a user"; }
        }

        public static string Kick(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    _Server.BanPlayer.KickUser(_Player.Get(args[i]).GameObject, "Unspecified", "Remote Connection");
                }
                return "User(s) kicked.";
            }
            catch { return "An error occured while kicking a user"; }
        }
    }
}
