using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public interface IInitializableValidator<TRuleAttribute> : IValidator where TRuleAttribute : Attribute, IRule
    {
        void Initialize(TRuleAttribute parameters);
    }
}
