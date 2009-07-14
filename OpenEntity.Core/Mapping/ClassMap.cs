using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OpenEntity.Helpers;
using System.Reflection;

namespace OpenEntity.Mapping
{
    public class ClassMap<TClass> : IClassMap
    {
        private IList<PropertyMapping> propertyMappings = new List<PropertyMapping>();
        
        /// <summary>
        /// The name of the table mapped.
        /// </summary>
        public string Table { get; set; }

        public Type ClassType { get { return typeof(TClass); } }

        /// <summary>
        /// Sets the table name for this class map.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void ForTable(string tableName)
        {
            this.Table = tableName;
        }

        /// <summary>
        /// Maps the property returned in the specified expression to the column of the same name.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public PropertyMapping Map(Expression<Func<TClass, object>> expression)
        {
            return this.Map(expression, null);
        }

        /// <summary>
        /// Maps the property returned in the specified expression to the specified column name.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public PropertyMapping Map(Expression<Func<TClass, object>> expression, string columnName)
        {
            return this.Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        /// <summary>
        /// Maps the specified property to the specified column name.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        protected virtual PropertyMapping Map(PropertyInfo property, string columnName)
        {
            var propertyMapping = new PropertyMapping()
            {
                Name = property.Name,
                PropertyInfo = property,
                Column = string.IsNullOrEmpty(columnName) ? property.Name : columnName
            };
            this.propertyMappings.Add(propertyMapping);
            return propertyMapping;
        }
    }
}
