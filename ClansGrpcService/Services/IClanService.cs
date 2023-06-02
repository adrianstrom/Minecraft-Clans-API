using Database.Models;

namespace ApiSite.Services
{
    public interface IClanService
    {
        public Task<(bool created, string? validationErrors)> AddClan(Clan clan);
        public Task<(bool deleted, string? validationErrors)> DeleteClan(string clanName, string playerId);
        public Task SetLeader(string playerId, string leaderId);
        public Task<(bool added, string? validationErrors)> AddClanMember(string clanName, string clanMemberId, string playerToAddId);
        public Task<(bool removed, string? validationErrors)> RemoveClanMember(string clanName, string clanMemberId, string playerToRemoveId);
        public Task<string> GetClanInfo(string clanName);
        // Add home
        // Remove home
        // Get clan info
    }
}
