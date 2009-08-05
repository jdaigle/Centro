using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public interface IValidator
    {
        bool IsValid(object value, IValidatorContext validatorContext);
    }
}
