using Database.Models;
using FluentValidation;

namespace ApiSite.Validators
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator() 
        {
        }
    }
}
