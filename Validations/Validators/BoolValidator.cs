using FluentValidation.Validators;

namespace Munizoft.Identity.Validations.Validators
{
    public class BoolValidator : PropertyValidator
    {
        public BoolValidator()
            : base("Bool type {PropertyValue} is not a valid type")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool boolType = (bool)context.PropertyValue;

            if (boolType == false || boolType == true)
                return true;

            return false;
        }
    }
}