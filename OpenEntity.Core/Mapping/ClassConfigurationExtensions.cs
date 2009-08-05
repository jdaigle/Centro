using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OpenEntity.Helpers;
using OpenEntity.Extensions;

namespace OpenEntity.Mapping
{
    public static class ClassConfigurationExtensions
    {
        public static string GetColumnName<TClass, TPropertyType>(this IClassConfiguration classConfiguration, Expression<Func<TClass, TPropertyType>> expression)
        {
            var propertyName = ReflectionHelper.GetProperty(expression).Name;
            if (classConfiguration == null)
                throw new InvalidOperationException("Could not find class configuration for type " + typeof(TClass).Name);
            var property = classConfiguration.Properties.FirstOrDefault(p => p.Name.Matches(propertyName));
            if (property == null)
                throw new InvalidOperationException("Could not find property configuration for " + propertyName);
            return property.Column;
        }
    }
}

