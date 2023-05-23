using Database.Models;
using Database.Repositories.Interfaces;
using FluentValidation;

namespace ClansGrpcService.Validators
{
    public class ClanValidator : AbstractValidator<Clan>
    {
        public ClanValidator(IClanRepository clanRepository)
        {
            RuleFor(clan => clan.Name.Length).GreaterThan(3).WithMessage("Clan name must be longer than two characters.");
            RuleFor(clan => clan.Name).MustAsync(async (clanName, cancellation) =>
            {
                var clan = await clanRepository.GetClan(clanName);
                return clan == null;
            }).WithMessage((clan) => $"{clan.Name} already exists. Try with another clan name.");
        }
    }
}
