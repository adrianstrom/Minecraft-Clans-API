using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.Interfaces
{
    public interface IClanRepository
    {
        /// <summary>
        /// Creates new clan if a clan with the same name does not exist
        /// </summary>
        /// <param name="name">Name of clan</param>
        /// <param name="leader">Leader of clan</param>
        /// <param name="location">Location of clan</param>
        /// <returns>Auto generated Id on the database server</returns>
        public Task<int> AddClan(Clan clan);
        public Task<Clan?> GetClan(string name);
        public Task<IEnumerable<Clan>> GetClans();
        public Task<bool> UpdateClan(string name);
        public Task<bool> DeleteClan(string name);

        /// <summary>
        /// Adds member to clan
        /// </summary>
        /// <param name="clan"></param>
        /// <returns>True if member added to clan, false if not</returns>
        public Task<bool> AddClanMember(Clan clan);
        /// <summary>
        /// Removes member from clan
        /// </summary>
        /// <param name="clan"></param>
        /// <returns>True if member removed from clan, false if not</returns>
        public Task<bool> RemoveClanMember(Clan clan);
    }
}
