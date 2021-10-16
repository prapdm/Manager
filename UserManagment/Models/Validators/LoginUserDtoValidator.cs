using FluentValidation;
using Manager.Entities;
using System.Linq;


namespace Manager.Models.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator(ManagerDbContext dBContext)
        {

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
        
    }
}
