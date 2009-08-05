using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace OpenEntity.Validation.Rules
{
    public class NotNullOrEmptyValidator : IValidator
    {
        public bool IsValid(object value, IValidatorContext validatorContext)
        {
            IEnumerable enumerable = value as IEnumerable;
            return value != null && (enumerable == null || enumerable.GetEnumerator().MoveNext());
        }
    }
}
