using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Mapping
{
    internal class PropertyMapping : IPropertyMapping
    {
        internal PropertyMapping(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Column = propertyInfo.Name;
        }

        public string Name { get { return PropertyInfo.Name; } }
        public PropertyInfo PropertyInfo { get; private set; }
        public string Column { get; private set; }
        public ICustomTypeConverter CustomTypeConverter { get; private set; }
        public bool HasReference { get { return Reference != null; } }
        public IReferenceMapping Reference { get; private set; }

        public IPropertyMapping AsColumn(string name)
        {
            Column = name;
            return this;
        }

        public IPropertyMapping AsCustomType(Type typeConverter)
        {
            if (!typeof(ICustomTypeConverter).IsAssignableFrom(typeConverter))
                throw new ArgumentException("Type converter must be of type ICustomTypeConverter", "typeConverter");
            CustomTypeConverter = Activator.CreateInstance(typeConverter) as ICustomTypeConverter;
            return this;
        }

        public IPropertyMapping References(IReferenceMapping reference)
        {
            Reference = reference;
            return this;
        }
    }
}
