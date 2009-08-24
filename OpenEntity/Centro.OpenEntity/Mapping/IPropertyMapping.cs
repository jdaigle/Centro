using System;
using System.Reflection;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Mapping
{
    public interface IPropertyMapping
    {
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        string Column { get; }
        ICustomTypeConverter CustomTypeConverter { get; }
        bool HasReference { get; }
        IReferenceMapping Reference { get; }
        bool HasOneToMany { get; }
        IOneToManyMapping OneToMany { get; }

        IPropertyMapping AsColumn(string name);
        IPropertyMapping AsCustomType(Type typeConverter);
        IPropertyMapping References(IReferenceMapping reference);
        IPropertyMapping HasMany(IOneToManyMapping many);
    }
}
