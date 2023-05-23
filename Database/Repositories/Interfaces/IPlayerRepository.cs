using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        public Task<bool> AddPlayer(Player player);
        public Task<Player?> GetPlayer(string id);
        public Task<IEnumerable<Player>> GetPlayers();
        public Task<bool> SetMemberOfClan(string playerId, int clanId);
    }
}
