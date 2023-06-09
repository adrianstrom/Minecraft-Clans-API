﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Player
    {
        /// <summary>
        /// The UUID of the player
        /// </summary>
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int? ClanId { get; set; }
        public Clan Clan { get; set; }
    }
}
