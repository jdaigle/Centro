using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenEntity.Validation.Rules
{
    public class PatternValidator : IInitializableValidator<PatternAttribute>, IValidator
    {
        private Regex regex;
        public virtual void Initialize(PatternAttribute parameters)
        {
            this.regex = new Regex(parameters.Regex, parameters.Flags);
        }
        public virtual bool IsValid(object value, IValidatorContext validatorContext)
        {
            return ((value == null) || this.regex.IsMatch(value.ToString()));
        }
    }

}
