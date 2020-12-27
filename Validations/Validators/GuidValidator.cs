using FluentValidation.Validators;
using System;

namespace Munizoft.Identity.Validations.Validators
{
    public class GuidValidator : PropertyValidator
    {
        public GuidValidator()
            : base("Guid type {PropertyValue} is not a valid type")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return false;

            Guid guidValue = (Guid)context.PropertyValue;
            Guid guid = Guid.NewGuid();

            if (Guid.TryParseExact(guidValue.ToString("B"), "B", out guid))
                return true;

            return false;
        }
    }
}