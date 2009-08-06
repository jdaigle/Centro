using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OpenEntity.Model;

namespace OpenEntity.Mapping
{
    public class PropertyConfiguration
    {
        internal PropertyConfiguration()
        {
        }
        public string Name { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public string Column { get; set; }
        public ICustomTypeConverter CustomTypeConverter { get; private set; }
        public PropertyConfiguration CustomType(Type typeConverter)
        {
            if (!typeof(ICustomTypeConverter).IsAssignableFrom(typeConverter))
                throw new ArgumentException("Type converter must be of type ICustomTypeConverter", "typeConverter");
            CustomTypeConverter = Activator.CreateInstance(typeConverter) as ICustomTypeConverter;
            return this;
        }
    }
}
