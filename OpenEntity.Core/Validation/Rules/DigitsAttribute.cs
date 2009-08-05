using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [ValidatorClass(typeof(DigitsValidator))]
    public class DigitsAttribute : Attribute, IRule
    {
        public DigitsAttribute()
        {
        }
        public DigitsAttribute(int integerDigits)
        {
            IntegerDigits = integerDigits;
        }
        public DigitsAttribute(int integerDigits, int fractionalDigits)
        {
            IntegerDigits = integerDigits;
            FractionalDigits = fractionalDigits;
        }
        public int FractionalDigits { get; set; }
        public int IntegerDigits { get; set; }
        public string Message { get; set; }
    }

}
