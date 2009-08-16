using System;
using System.Reflection;
using OpenEntity.Model;

namespace OpenEntity.Mapping
{
    public interface IPropertyMapping
    {
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        string Column { get; }
        ICustomTypeConverter CustomTypeConverter { get; }
        bool HasReference { get; }
        IReferenceMapping Reference { get; }

        IPropertyMapping AsColumn(string name);
        IPropertyMapping AsCustomType(Type typeConverter);
        IPropertyMapping References(IReferenceMapping reference);
    }
}
