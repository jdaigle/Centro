using System;
using NHibernate.Validator.Engine;

namespace Centro.Data.Validation.Rules
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [ValidatorClass(typeof(UtcDateTimeValidator))]
    public class UtcDateTimeAttribute : Attribute, IRuleArgs
    {
        private string message = "The date 'Kind' must be UTC.";
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
