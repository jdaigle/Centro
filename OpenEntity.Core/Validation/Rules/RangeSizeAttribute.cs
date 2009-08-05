using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    //[ValidatorClass(typeof(CollectionSizeValidator))]
    public class RangeAttribute : Attribute, IRule
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public string Message { get; set; }
    }
}
