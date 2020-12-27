using FluentValidation;
using Munizoft.Identity.Resources.Auth;
using Munizoft.Identity.Validations.Validators;

namespace Munizoft.Identity.Validations
{
    public class LogoutRequestValidator : AbstractValidator<LogoutRequestResource>
    {
        public LogoutRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .SetValidator(new GuidValidator())
                ;

        }
    }
}