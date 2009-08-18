using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Centro.OpenEntity.Validation
{
    public static class DataAnnotationsValidationRunner
    {
        public static IList<ErrorInfo> GetErrors(object instance)
        {
            var allErrors = new List<ErrorInfo>();
            foreach (var property in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>())
            {
                allErrors.AddRange(property.Attributes
                                           .OfType<ValidationAttribute>()
                                           .Where(x => !x.IsValid(property.GetValue(instance)))
                                           .Select(x => new ErrorInfo(property.Name, x.FormatErrorMessage(string.Empty), instance)));
            }
            return allErrors;
        }
    }
}
