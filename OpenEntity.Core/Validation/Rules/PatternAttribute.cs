using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [ValidatorClass(typeof(PatternValidator))]
    public class PatternAttribute : Attribute, IRule
    {
        public PatternAttribute()
        {
        }
        public PatternAttribute(string regex)
        {
            Regex = regex;
        }
        public PatternAttribute(string regex, RegexOptions flags)
        {
            Regex = regex;
            Flags = flags;
        }
        public PatternAttribute(string regex, RegexOptions flags, string message)
        {
            Regex = regex;
            Flags = flags;
            Message = message;
        }

        public RegexOptions Flags { get; set; }
        public string Regex { get; set; }
        public string Message { get; set; }
    }
}
