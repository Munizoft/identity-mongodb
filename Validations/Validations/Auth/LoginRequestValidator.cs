using FluentValidation;
using Munizoft.Identity.Resources.Auth;

namespace Munizoft.Identity.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestResource>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                ;

            RuleFor(x => x.Password)
              .NotNull()
              .NotEmpty()
              ;

        }
    }
}
