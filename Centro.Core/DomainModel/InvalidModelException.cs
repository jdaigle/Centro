using System;
using System.Collections.Generic;
using Centro.Validation;

namespace Centro.DomainModel
{
    public class InvalidModelException : DomainModelException
    {
        public InvalidModelException(IEnumerable<ValidationError> validationErrors)
            : this(validationErrors, null)
        {
        }

        public InvalidModelException(IEnumerable<ValidationError> validationErrors, Exception innerException)
            : base("Model Failed Validation", innerException)
        {
            ValidationErrors = validationErrors;
        }

        public InvalidModelException(string propertyName, string errorMessage)
            : this(propertyName, errorMessage, null, null)
        {
        }

        public InvalidModelException(string propertyName, string errorMessage, object onObject)
            : this(propertyName, errorMessage, onObject, null)
        {
        }

        public InvalidModelException(string propertyName, string errorMessage, object onObject, Exception innerException)
            : base("Model Failed Validation", innerException)
        {
            ValidationErrors = new[] { new ValidationError(propertyName, errorMessage, onObject) };
        }

        public IEnumerable<ValidationError> ValidationErrors { get; private set; }
    }
}
