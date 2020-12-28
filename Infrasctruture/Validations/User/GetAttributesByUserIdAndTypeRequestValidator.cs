using FluentValidation;
using Munizoft.Identity.Resources.User;
using Munizoft.Identity.Validations.Validators;

namespace Munizoft.Identity.Infrastructure.Validations.User
{
    public class GetByIdRequestValidatior<TKey> : AbstractValidator<GetAttributesByUserIdAndTypeRequestResource<TKey>>
    {
        public GetByIdRequestValidatior()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .SetValidator(new GuidValidator())
                ;

            RuleFor(x => x.Type)
               .NotNull()
               .NotEmpty()
               ;
        }
    }
}