using System;
using Centro.Validation;
using NHibernate.Validator.Engine;

namespace Centro.Data.Validation.Rules
{
    public class UtcDateTimeValidator : NHibernate.Validator.Engine.IValidator
    {
        public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
        {
            if (value == null)
                return true;
            try
            {
                if (((DateTime)value).Kind != DateTimeKind.Utc)
                    return false;
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(
                    "The UtcDateTimeValidator may only be used against DateTime or DateTime? properties.");
            }
            return true;
        }
    }
}
