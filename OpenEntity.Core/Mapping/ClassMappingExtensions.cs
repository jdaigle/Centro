using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OpenEntity.Helpers;
using OpenEntity.Extensions;

namespace OpenEntity.Mapping
{
    public static class ClassMappingExtensions
    {
        public static string GetColumnName<TClass>(this IClassMapping classMapping, Expression<Func<TClass, object>> expression)
        {
            var propertyName = ReflectionHelper.GetProperty(expression).Name;
            if (classMapping == null)
                throw new InvalidOperationException("Could not find class mapping for type " + typeof(TClass).Name);
            var propertyMapping = classMapping.PropertyMappings.FirstOrDefault(p => p.Name.Matches(propertyName));
            if (propertyMapping == null)
                throw new InvalidOperationException("Could not find property mapping for " + propertyName);
            return propertyMapping.Column;
        }
    }
}

