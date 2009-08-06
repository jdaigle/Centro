using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OpenEntity.Model;

namespace OpenEntity.Mapping
{
    internal class PropertyConfiguration : IPropertyConfiguration
    {
        internal PropertyConfiguration(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Column = propertyInfo.Name;
        }

        public string Name { get { return PropertyInfo.Name; } }
        public PropertyInfo PropertyInfo { get; private set; }
        public string Column { get; private set; }
        public ICustomTypeConverter CustomTypeConverter { get; private set; }

        public IPropertyConfiguration AsColumn(string name)
        {
            Column = name;
            return this;
        }

        public IPropertyConfiguration AsCustomType(Type typeConverter)
        {
            if (!typeof(ICustomTypeConverter).IsAssignableFrom(typeConverter))
                throw new ArgumentException("Type converter must be of type ICustomTypeConverter", "typeConverter");
            CustomTypeConverter = Activator.CreateInstance(typeConverter) as ICustomTypeConverter;
            return this;
        }
    }
}
