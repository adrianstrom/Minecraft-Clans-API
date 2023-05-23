using ClansGrpcService.Protos;
using Database.Models;
using Database.Repositories.Interfaces;
using FluentValidation;
using Grpc.Core;

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

        public override Task<GetClanResponse> GetClan(GetClanRequest request, ServerCallContext context)
        {
            return null;
        }

        public override Task<UpdateClanResponse> UpdateClan(UpdateClanRequest request, ServerCallContext context)
        {
            return null;
        }

        public override Task<DeleteClanResponse> DeleteClan(DeleteClanRequest request, ServerCallContext context)
        {
            return null;
        }

    }
}
