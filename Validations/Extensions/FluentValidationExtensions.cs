using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Munizoft.Identity.Validations.Extensions
{
    public static class FluentValidationExtensions
    {
        public static ValidationResult Validate<TValidator, TEntity>(this TEntity source)
          where TValidator : AbstractValidator<TEntity>
        {
            return Activator.CreateInstance<TValidator>().Validate(source);
        }

        public static List<String> ToList(this ValidationResult source)
        {
            List<String> errors = new List<string>();
            foreach (var error in source.Errors)
            {
                errors.Add(error.ErrorMessage);
            }

            return errors;
        }

        public static String[] ToArray(this ValidationResult source)
        {
            return source != null && source.Errors != null && source.Errors.Any() ? source.ToArray() : null;
        }

        //public static IRuleBuilderOptions<T, String> Role<T>(this IRuleBuilder<T, String> rule)
        //{
        //    return rule.SetValidator(new RoleValidator());
        //}
    }
}
