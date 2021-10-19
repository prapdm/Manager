using FluentValidation;
using Manager.Entities;


namespace Manager.Models.Validators
{
    public class VeryfiyEmailDtoValidator : AbstractValidator<VeryfiyEmailDto>
    {
        public VeryfiyEmailDtoValidator()
        {

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.VerifcationCode).NotEmpty();

        }
    }
}
