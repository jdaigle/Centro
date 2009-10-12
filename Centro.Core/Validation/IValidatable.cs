using System.Collections.Generic;

namespace Centro.Validation
{
    public interface IValidatable
    {
        bool IsValid();
        IEnumerable<ValidationError> ValidationErrors();
    }
}
