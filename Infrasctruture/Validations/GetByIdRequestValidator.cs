using FluentValidation;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Validations.Validators;

namespace Munizoft.Identity.Infrastructure.Validations.User
{
    public class GetByIdRequestValidator<TKey> : AbstractValidator<GetByIdRequest<TKey>>
    {
        public GetByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .SetValidator(new GuidValidator())
                ;
        }
    }
}