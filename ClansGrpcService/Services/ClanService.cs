using ClansGrpcService.Protos;
using Database.Models;
using Database.Repositories.Interfaces;
using FluentValidation;
using Grpc.Core;
using System.Linq;

namespace ClansGrpcService.Services
{
    public class ClanService : Protos.Clan.ClanBase
    {
        private readonly ILogger<ClanService> _logger;
        private IClanRepository _clanRepository;
        private IPlayerRepository _playerRepository;
        private IValidator<Database.Models.Clan> _clanValidator;
        private IValidator<Player> _playerValidator;

        public ClanService(
            IClanRepository clanRepository, 
            IPlayerRepository playerRepository,
            IValidator<Database.Models.Clan> clanValidator,
            IValidator<Player> playerValidator,
            ILogger<ClanService> logger)
        {
            _clanRepository = clanRepository;
            _playerRepository = playerRepository;
            _playerValidator = playerValidator;
            _clanValidator = clanValidator;
            _logger = logger;
        }

        public override async Task<AddClanResponse> AddClan(AddClanRequest request, ServerCallContext context)
        {
            var clan = new Database.Models.Clan();
            clan.Name = request.Name;
            clan.Leader = request.Leader;
            clan.Location = new Database.Models.Location()
            {
                World = request.Location.World,
                X = request.Location.X,
                Y = request.Location.Y,
                Z = request.Location.Z,
                Pitch = request.Location.Pitch,
                Yaw = request.Location.Yaw,
            };

            var validationResult = await _clanValidator.ValidateAsync(clan);
            if (!validationResult.IsValid)
            {
                return new AddClanResponse() { Message = validationResult.Errors.ToString() };
            }

            // Validation succeeded, so add clan and set this as the active clan for the player.
            var clanId = await _clanRepository.AddClan(clan);
            await _playerRepository.SetMemberOfClan(clan.Leader, clanId);
            return new AddClanResponse();
        }

        public async override Task<GetClanResponse> GetClan(GetClanRequest request, ServerCallContext context)
        {
            var clanName = request.ClanName;
            var clan = await _clanRepository.GetClan(clanName, true, true, true);

            if (clan == null) 
            {
                return new GetClanResponse() { Message = $"The clan with name {clanName} does not exist" };
            }
            var location = clan.Location;
            var message = $@"Clan: {clan.Name}
                             Leader: {clan.Leader}
                             Location: {location?.World} ({(int)location?.X}, {(int)location?.Y}, {(int)location?.Z})
                             Members: {string.Join(", ", clan.Members)}";
            return new GetClanResponse() { Message = message };
        }

        public override Task<UpdateClanResponse> UpdateClan(UpdateClanRequest request, ServerCallContext context)
        {
            return null;
        }

        public async override Task<DeleteClanResponse> DeleteClan(DeleteClanRequest request, ServerCallContext context)
        {
            var clanName = request.Name;
            var playerId = request.PlayerId;

            var clan = await _clanRepository.GetClan(clanName);

            // Delete the clan only if it exists and the player is the leader of the clan
            if (clan != null && clan.Leader == playerId)
            {
                await _clanRepository.DeleteClan(request.Name);
                return new DeleteClanResponse() { Message = $"Successfully deleted the clan with name {clanName}" };
            } 
            else if (clan == null) 
            {
                return new DeleteClanResponse() { Message =  $"The clan with name {clanName} which you are trying to delete does not exist" };
            }
            return new DeleteClanResponse() { Message = $"Could not delete the clan with name {clanName} because you are not the leader of this clan"};
        }

        public async override Task<AddClanMemberResponse> AddClanMember(AddClanMemberRequest request, ServerCallContext context)
        {
            var clanMemberId = request.ClanMemberId;
            var playerToAddId = request.PlayerToAddId;

            var clanMember = await _playerRepository.GetPlayer(clanMemberId);
            if (!clanMember.ClanId.HasValue)
            {
                return new AddClanMemberResponse() { Message = "You are not part of any clan" };
            }

            var playerToAdd = await _playerRepository.GetPlayer(playerToAddId);
            if (playerToAdd == null)
            {
                return new AddClanMemberResponse() { Message = "The player you are trying to add does not exist. Try again" };
            } 

            if (playerToAdd.ClanId.HasValue)
            {
                return new AddClanMemberResponse() { Message = "The player you are trying to add is already part of another clan" };
            }

            await _clanRepository.AddClanMember(playerToAddId, clanMember.ClanId.Value);
            return new AddClanMemberResponse() { Message = "Added" };
        }

        public async override Task<RemoveClanMemberResponse> RemoveClanMember(RemoveClanMemberRequest request, ServerCallContext context)
        {
            var clanMemberId = request.ClanMemberId;
            var playerToRemoveId = request.PlayerToRemoveId;

            var clanMember = await _playerRepository.GetPlayer(clanMemberId);
            if (!clanMember.ClanId.HasValue)
            {
                return new RemoveClanMemberResponse() { Message = "You are not part of any clan" };
            }

            var playerToRemove = await _playerRepository.GetPlayer(playerToRemoveId);
            if (playerToRemove == null)
            {
                return new RemoveClanMemberResponse() { Message = "The player you are trying to remove does not exist. Try again" };
            }

            if (!playerToRemove.ClanId.HasValue)
            {
                return new RemoveClanMemberResponse() { Message = "The player you are trying to remove is not part of any clan" };
            }
            await _clanRepository.RemoveClanMember(playerToRemoveId, clanMember.ClanId.Value);

            return new RemoveClanMemberResponse() { Message = "Removed" };
        }
    }
}
