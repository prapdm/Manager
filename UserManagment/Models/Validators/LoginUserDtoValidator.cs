using FluentValidation;
using Manager.Entities;



namespace Manager.Models.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
        
    }
}
