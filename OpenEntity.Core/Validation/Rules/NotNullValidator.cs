using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    public class NotNullValidator : IValidator
    {
        public bool IsValid(object value, IValidatorContext validatorContext)
        {
            return (value != null);
        }
    }
}
