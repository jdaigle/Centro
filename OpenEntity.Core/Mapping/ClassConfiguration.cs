using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OpenEntity.Helpers;
using System.Reflection;
using OpenEntity.Extensions;

namespace OpenEntity.Mapping
{
    public class ClassConfiguration<TClass> : IClassConfiguration
    {
        private IList<PropertyConfiguration> properties = new List<PropertyConfiguration>();
        
        public string Table { get; set; }
        public Type ClassType { get { return typeof(TClass); } }
        public IList<PropertyConfiguration> Properties { get { return this.properties; } }

        public void ForTable(string tableName)
        {
            this.Table = tableName;
        }

        public PropertyConfiguration Map<TResult>(Expression<Func<TClass, TResult>> expression)
        {
            return this.Map(expression, null);
        }

        public PropertyConfiguration Map<TResult>(Expression<Func<TClass, TResult>> expression, string columnName)
        {
            return this.Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual PropertyConfiguration Map(PropertyInfo property, string columnName)
        {
            var propertyConfig = new PropertyConfiguration()
            {
                Name = property.Name,
                PropertyInfo = property,
                Column = string.IsNullOrEmpty(columnName) ? property.Name : columnName
            };
            this.properties.Add(propertyConfig);
            return propertyConfig;
        }
    }
}
