﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public static class ClanExtensions
    {
        public static object GetParameters(this Clan clan)
        {
            return 
                new
                {
                    Name = clan.Name,
                    Leader = clan.Leader,
                    World = clan.Location.World,
                    X = clan.Location.X,
                    Y = clan.Location.Y,
                    Z = clan.Location.Z,
                    Yaw = clan.Location.Yaw,
                    Pitch = clan.Location.Pitch,
            };
        }
    }

    public class Clan
    {
        public int Id { get; set; }
        /// <summary>
        /// Name of the clan
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The PlayerId of the leader of the clan
        /// </summary>
        public string Leader { get; set; }
        public Location Location { get; set; }
        public IEnumerable<Location> Homes { get; set; }
        public IEnumerable<Player> Members { get; set; }
    }
}
