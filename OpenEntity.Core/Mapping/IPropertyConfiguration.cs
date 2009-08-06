using System;
using System.Reflection;
using OpenEntity.Model;

namespace OpenEntity.Mapping
{
    public interface IPropertyConfiguration
    {
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        string Column { get; }
        ICustomTypeConverter CustomTypeConverter { get; }

        IPropertyConfiguration AsColumn(string name);
        IPropertyConfiguration AsCustomType(Type typeConverter);
    }
}
