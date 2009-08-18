using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Model
{
    public interface ICustomTypeConverter
    {
        object ConvertTo(object value);
        object ConvertFrom(object value);
    }
}
