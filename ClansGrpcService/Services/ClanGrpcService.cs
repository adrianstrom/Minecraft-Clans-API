using ApiSite.Protos;
using Database.Models;
using Database.Repositories.Interfaces;
using FluentValidation;
using Grpc.Core;
using System.Linq;

namespace ApiSite.Services
{
    public class ClanGrpcService : Protos.Clan.ClanBase
    {
        private readonly ClanService _clanService;
        private readonly ILogger<ClanGrpcService> _logger;

        public ClanGrpcService(
            ClanService clanService,
            ILogger<ClanGrpcService> logger)
        {
            _clanService = clanService;
            _logger = logger;
        }

        public override async Task<AddClanResponse> AddClan(AddClanRequest request, ServerCallContext context)
        {
            var clan = Mapper.ClanMapper.ToDbClan(request);
            var (created, validationErrors) = await _clanService.AddClan(clan);

            if (!created)
            {
                return new AddClanResponse() { Message = validationErrors };
            }
            return new AddClanResponse() { Message = "Clan created" };
        }

        public async override Task<GetClanResponse> GetClan(GetClanRequest request, ServerCallContext context)
        {
            var clanInfo = await _clanService.GetClanInfo(request.ClanName);
            return new GetClanResponse() { Message = clanInfo };
        }

        public override Task<UpdateClanResponse> UpdateClan(UpdateClanRequest request, ServerCallContext context)
        {
            return null;
        }

        public async override Task<DeleteClanResponse> DeleteClan(DeleteClanRequest request, ServerCallContext context)
        {
            var (deleted, validationErrors) = await _clanService.DeleteClan(request.Name, request.PlayerId);
            if (!deleted)
            {
                return new DeleteClanResponse() { Message = validationErrors };
            }
            return new DeleteClanResponse() { Message = "Clan deleted" };
        }

        public async override Task<AddClanMemberResponse> AddClanMember(AddClanMemberRequest request, ServerCallContext context)
        {
            var (added, validationErors) = await _clanService.AddClanMember(request.ClanName, request.ClanMemberId, request.PlayerToAddId);
            if (!added)
            {
                return new AddClanMemberResponse() { Message = validationErors };
            }
            return new AddClanMemberResponse() { Message = "Added" };
        }

        public async override Task<RemoveClanMemberResponse> RemoveClanMember(RemoveClanMemberRequest request, ServerCallContext context)
        {
            var (removed, validationErors) = await _clanService.RemoveClanMember(request.ClanName, request.ClanMemberId, request.PlayerToRemoveId);
            if (!removed)
            {
                return new RemoveClanMemberResponse() { Message = validationErors };
            }
            return new RemoveClanMemberResponse() { Message = "Removed" };
        }
    }
}
