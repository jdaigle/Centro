using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    //[ValidatorClass(typeof(FutureValidator))]
    public class MaxAttribute : Attribute, IRule
    {
        public int Limit { get; set; }
        public string Message { get; set; }
    }
}
