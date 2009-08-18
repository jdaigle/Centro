using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Specs.Mock
{
    public class SimplePhoneNumberConverter : ICustomTypeConverter
    {
        public object ConvertTo(object value)
        {
            var stringValue = value as string;
            if (stringValue == null)
                throw new NotSupportedException("Cannot convert from a non-string value.");
            return new SimplePhoneNumber(stringValue);
        }

        public object ConvertFrom(object value)
        {
            return (value != null) ? value.ToString() : null;
        }
    }
}
