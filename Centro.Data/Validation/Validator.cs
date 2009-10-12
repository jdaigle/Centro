using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;
using Centro.Validation;

namespace Centro.Data.Validation
{
    public class Validator : Centro.Validation.IValidator
    {
        private readonly ValidatorEngine validatorEngine;

        public Validator(ValidatorEngine validatorEngine)
        {
            this.validatorEngine = validatorEngine;
        }

        public bool IsValid(object value)
        {
            return validatorEngine.IsValid(value);
        }

        public IEnumerable<ValidationError> ValidationErrorsFor(object value)
        {
            return validatorEngine.Validate(value).Select(x => new ValidationError(x.PropertyName, x.Message, x.Entity)).ToList();
        }
    }
}
