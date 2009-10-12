using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.Validation
{
    public class ValidationError
    {
        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; }
        public object Object { get; private set; }
 
        public ValidationError(string propertyName, string errorMessage, object onObject)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Object = onObject;
        }

        public ValidationError(string propertyName, string errorMessage)
            : this(propertyName, errorMessage, null)
        {
        }
    }
}
