using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public class ValidatorClassAttribute : Attribute
    {
        public ValidatorClassAttribute(string fullAssemblyQualifyName)
        {
            Value = Type.GetType(fullAssemblyQualifyName, false);
        }

        public ValidatorClassAttribute(Type value)
        {
            Value = value;
        }

        public Type Value { get; private set; }
    }
}
