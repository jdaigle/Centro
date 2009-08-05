using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [ValidatorClass(typeof(NotNullOrEmptyValidator))]
    public class NotNullOrEmptyAttribute : Attribute, IRule
    {
        public string Message { get; set; }
    }
}
