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
        private IList<IPropertyConfiguration> properties = new List<IPropertyConfiguration>();
        
        public string Table { get; private set; }
        public Type ClassType { get { return typeof(TClass); } }
        public IList<IPropertyConfiguration> Properties { get { return this.properties; } }

        public void ForTable(string tableName)
        {
            this.Table = tableName;
        }

        public IPropertyConfiguration Maps<TResult>(Expression<Func<TClass, TResult>> propertyExpression)
        {
            var property = ReflectionHelper.GetProperty(propertyExpression);
            var propertyConfig = new PropertyConfiguration(property);
            this.properties.Add(propertyConfig);
            return propertyConfig;
        }
    }
}
