using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace OpenEntity.Validation.Rules
{
    public class IsNumericValidator : IValidator
    {
        public bool IsValid(object value, IValidatorContext validatorContext)
        {
            double num;
            bool isNumberic = double.TryParse(Convert.ToString(value), NumberStyles.Any, (IFormatProvider)NumberFormatInfo.InvariantInfo, out num);
            return (value == null) || ((value is string) && isNumberic);            
        }
    }   
}
