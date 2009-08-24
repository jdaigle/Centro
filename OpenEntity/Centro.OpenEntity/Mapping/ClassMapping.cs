using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Centro.OpenEntity.Helpers;
using System.Reflection;
using Centro.OpenEntity.Extensions;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Mapping
{
    public class ClassMapping<TClass> : IClassMapping
    {
        private IList<IPropertyMapping> properties = new List<IPropertyMapping>();
        
        public string Table { get; private set; }
        public Type ClassType { get { return typeof(TClass); } }
        public IList<IPropertyMapping> Properties { get { return this.properties; } }

        public void ForTable(string tableName)
        {
            this.Table = tableName;
        }

        public IPropertyMapping Maps<TResult>(Expression<Func<TClass, TResult>> propertyExpression)
        {
            var property = ReflectionHelper.GetProperty(propertyExpression);
            var propertyMapping = new PropertyMapping(property);
            this.properties.Add(propertyMapping);
            return propertyMapping;
        }

        public IReferenceMapping References<TModelType>(Expression<Func<TClass, TModelType>> propertyExpression)
            where TModelType : IDomainObject
        {
            var propertyMapping = this.Maps(propertyExpression);
            var referenceMapping = new ReferenceMapping(propertyMapping, typeof(TModelType));
            propertyMapping.References(referenceMapping);
            return referenceMapping;
        }

        public IOneToManyMapping HasMany<TModelCollectionType, TModelType>(Expression<Func<TClass, TModelCollectionType>> propertyExpression)
            where TModelCollectionType : IEnumerable<TModelType>
            where TModelType : IDomainObject
        {
            var propertyMapping = this.Maps(propertyExpression);
            var oneToManyMapping = new OneToManyMapping(propertyMapping, typeof(TModelType));
            propertyMapping.HasMany(oneToManyMapping);
            return oneToManyMapping;
        }
    }
}
