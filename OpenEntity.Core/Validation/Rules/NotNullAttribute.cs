using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [ValidatorClass(typeof(NotNullValidator))]
    public class NotNullAttribute : Attribute, IRule
    {
        public string Message { get; set; }
    }
}
