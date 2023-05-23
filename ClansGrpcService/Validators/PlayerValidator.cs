using Database.Models;
using FluentValidation;

namespace ClansGrpcService.Validators
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator() 
        {
        }
    }
}
