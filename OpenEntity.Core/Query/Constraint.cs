using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OpenEntity.Helpers;
using OpenEntity.Mapping;
using OpenEntity.Extensions;
using OpenEntity.DataProviders;

namespace OpenEntity.Query
{
    public class Constraint<TEntity> : IPredicate, IConstraint
    {
        private ColumnConstraint columnConstraint;

        public Constraint(Expression<Func<TEntity, object>> expression)
        {
            var propertyName = ReflectionHelper.GetProperty(expression).Name;
            var classMapping = MappingConfig.FindClassMapping(typeof(TEntity));
            if (classMapping == null)
                throw new InvalidOperationException("Could not find class mapping for type " + typeof(TEntity).Name);
            var propertyMapping = classMapping.PropertyMappings.FirstOrDefault(p => p.Name.Matches(propertyName));
            if (propertyMapping == null)
                throw new InvalidOperationException("Could not find property mapping for " + propertyName);
            columnConstraint = new ColumnConstraint(classMapping.Table, propertyMapping.Column);
        }

        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker)
        {
            return columnConstraint.ToQueryText(dataProvider, ref uniqueMarker);
        }

        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker, bool inHavingClause)
        {
            return columnConstraint.ToQueryText(dataProvider, ref uniqueMarker, inHavingClause);
        }

        public IList<System.Data.IDataParameter> Parameters
        {
            get { return columnConstraint.Parameters; }
        }

        public bool Negate
        {
            get
            {
                return columnConstraint.Negate;
            }
            set
            {
                columnConstraint.Negate = value;
            }
        }
        public IConstraint IsLike(string value)
        {
            return columnConstraint.IsLike(value);
        }

        public IConstraint IsNotLike(string value)
        {
            return columnConstraint.IsNotLike(value);
        }

        public IConstraint IsGreaterThan(object value)
        {
            return columnConstraint.IsGreaterThan(value);
        }

        public IConstraint IsGreaterThanOrEqualTo(object value)
        {
            return columnConstraint.IsGreaterThanOrEqualTo(value);
        }

        public IConstraint IsIn(System.Collections.IEnumerable values)
        {
            return columnConstraint.IsIn(values);
        }

        public IConstraint IsNotIn(System.Collections.IEnumerable values)
        {
            return columnConstraint.IsNotIn(values);
        }

        public IConstraint IsLessThan(object value)
        {
            return columnConstraint.IsLessThan(value);
        }

        public IConstraint IsLessThanOrEqualTo(object value)
        {
            return columnConstraint.IsLessThanOrEqualTo(value);
        }

        public IConstraint IsNotNull()
        {
            return columnConstraint.IsNotNull();
        }

        public IConstraint IsNull()
        {
            return columnConstraint.IsNull();
        }

        public IConstraint IsBetweenAnd(object value1, object value2)
        {
            return columnConstraint.IsBetweenAnd(value1, value2);
        }

        public IConstraint IsEqualTo(object value)
        {
            return columnConstraint.IsEqualTo(value);
        }

        public IConstraint IsNotEqualTo(object value)
        {
            return columnConstraint.IsNotEqualTo(value);
        }
    }
}
