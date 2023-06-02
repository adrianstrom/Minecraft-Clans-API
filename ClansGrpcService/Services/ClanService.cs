using Database.Models;
using Database.Repositories.Interfaces;
using FluentValidation;

namespace ApiSite.Services
{
    public class ClanService : IClanService
    {
        private readonly ILogger<ClanService> _logger;
        private IClanRepository _clanRepository;
        private IPlayerRepository _playerRepository;
        private IValidator<Clan> _clanValidator;
        private IValidator<Player> _playerValidator;

        public ClanService(
            IClanRepository clanRepository,
            IPlayerRepository playerRepository,
            IValidator<Clan> clanValidator,
            IValidator<Player> playerValidator,
            ILogger<ClanService> logger)
        {
            _clanRepository = clanRepository;
            _playerRepository = playerRepository;
            _playerValidator = playerValidator;
            _clanValidator = clanValidator;
            _logger = logger;
        }

        public async Task<(bool created, string? validationErrors)> AddClan(Clan clan)
        {
            var validationResult = await _clanValidator.ValidateAsync(clan);
            if (!validationResult.Errors.Any())
            {
                var clanId = await _clanRepository.AddClan(clan);
                if (clanId > 0)
                {
                    await _playerRepository.SetMemberOfClan(clan.Leader, clanId);
                    return (true, null);
                }
                return (false, null);
            }
            return (false, validationResult.Errors.ToString());
        }

        public async Task<(bool deleted, string? validationErrors)> DeleteClan(string clanName, string playerId)
        {
            var clan = await _clanRepository.GetClan(clanName);

            // Delete the clan only if it exists and the player is the leader of the clan (TODO: create delete clan validator)
            if (clan != null && clan.Leader == playerId)
            {
                await _clanRepository.DeleteClan(clanName);
                return (true, null);
            }
            else if (clan == null)
            {
                return (false, $"The clan with name {clanName} which you are trying to delete does not exist");
            }
            return (false, $"Could not delete the clan with name {clanName} because you are not the leader of this clan");
        }

        public async Task<(bool added, string? validationErrors)> AddClanMember(string clanName, string clanMemberId, string playerToAddId)
        {
            var clanMember = await _playerRepository.GetPlayer(clanMemberId);
            if (!clanMember.ClanId.HasValue)
            {
                return (false, "You are not part of any clan");
            }

            var playerToAdd = await _playerRepository.GetPlayer(playerToAddId);
            if (playerToAdd == null)
            {
                return (false, "The player you are trying to add does not exist. Try again");
            }

            if (playerToAdd.ClanId.HasValue)
            {
                return (false, "The player you are trying to add is already part of another clan");
            }

            await _clanRepository.AddClanMember(playerToAddId, clanMember.ClanId.Value);
            return (true, null);
        }

        public async Task<(bool removed, string? validationErrors)> RemoveClanMember(string clanName, string clanMemberId, string playerToRemoveId)
        {
            var clanMember = await _playerRepository.GetPlayer(clanMemberId);
            if (!clanMember.ClanId.HasValue)
            {
                return (false, "You are not part of any clan");
            }

            var playerToRemove = await _playerRepository.GetPlayer(playerToRemoveId);
            if (playerToRemove == null)
            {
                return (false, "The player you are trying to remove does not exist. Try again");
            }

            if (!playerToRemove.ClanId.HasValue)
            {
                return (false, "The player you are trying to remove is not part of any clan");
            }
            await _clanRepository.RemoveClanMember(playerToRemoveId, clanMember.ClanId.Value);
            return (true, null);
        }

        public async Task<string> GetClanInfo(string clanName)
        {
            var clan = await _clanRepository.GetClan(clanName, true, true, true);

            if (clan == null)
            {
                return $"The clan with name {clanName} does not exist";
            }
            var location = clan.Location;
            return        $@"Clan: {clan.Name}
                             Leader: {clan.Leader}
                             Location: {location.World} ({(int)location.X}, {(int)location.Y}, {(int)location.Z})
                             Members: {string.Join(", ", clan.Members)}";
        }

        public Task SetLeader(string playerId, string leaderId)
        {
            throw new NotImplementedException();
        }
    }
}
