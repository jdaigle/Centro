using System.Collections.Generic;

namespace Centro.Validation
{
    public interface IValidator
    {
        bool IsValid(object value);
        IEnumerable<ValidationError> ValidationErrorsFor(object value);
    }
}
