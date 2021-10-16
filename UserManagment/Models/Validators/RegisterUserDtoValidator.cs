using FluentValidation;
using System.Linq;
using Manager.Entities;

namespace Manager.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(ManagerDbContext dBContext)
        {

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(e => e.Password);
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dBContext.User.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }

                });
        }
    }
}
