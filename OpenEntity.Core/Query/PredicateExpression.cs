using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using OpenEntity.DataProviders;
using OpenEntity.Mapping;
using OpenEntity.Model;

namespace OpenEntity.Query
{
    public class PredicateExpression : IPredicateExpression
    {
        private List<PredicateExpressionElement> elements;
        private List<IDataParameter> parameters;

        public PredicateExpression()
        {
            this.parameters = new List<IDataParameter>();
            this.elements = new List<PredicateExpressionElement>();
        }

        public IPredicateExpression And(IPredicate predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate", "Predicate To Add is null");
            return this.AddPredicate(predicate, PredicateExpressionOperator.And);
        }

        public IPredicateExpression Or(IPredicate predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate", "Predicate To Add is null");
            return this.AddPredicate(predicate, PredicateExpressionOperator.Or);
        }

        public IConstraint Where<TModelType>(Expression<Func<TModelType, object>> columnExpression)
             where TModelType : IDomainObject
        {
            return Constrain<TModelType>(columnExpression, PredicateExpressionOperator.And);
        }

        public IConstraint And<TModelType>(Expression<Func<TModelType, object>> columnExpression)
             where TModelType : IDomainObject
        {
            return Constrain<TModelType>(columnExpression, PredicateExpressionOperator.And);
        }

        public IConstraint Or<TModelType>(Expression<Func<TModelType, object>> columnExpression)
             where TModelType : IDomainObject
        {
            return Constrain<TModelType>(columnExpression, PredicateExpressionOperator.Or);
        }

        private IConstraint Constrain<TModelType>(Expression<Func<TModelType, object>> columnExpression, PredicateExpressionOperator operatorToUse)
             where TModelType : IDomainObject
        {
            var classMapping = MappingConfig.FindClassMapping(typeof(TModelType));
            var tableName = classMapping.Table;
            var columnName = classMapping.GetColumnName<TModelType>(columnExpression);
            return new ColumnConstraint(tableName, columnName, this, operatorToUse);
        }

        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider", "Cannot create query part without a schema from the data repository.");
            }

            StringBuilder queryText = new StringBuilder(128);
            this.Parameters.Clear();

            // start with the opening paren
            if (this.Negate)
            {
                queryText.Append("NOT (");
            }
            else
            {
                queryText.Append("(");
            }

            for (int i = 0; i < this.elements.Count; i++)
            {
                PredicateExpressionElement element = this.elements[i];

                switch (element.Type)
                {
                    case PredicateExpressionElementType.Operator:
                        queryText.AppendFormat(null, " {0}", ((PredicateExpressionOperator)element.Contents).ToString().ToUpper(CultureInfo.InvariantCulture));
                        break;
                    case PredicateExpressionElementType.Predicate:
                        IPredicate predicateToAdd = (IPredicate)element.Contents;
                        // get the text of the subquery
                        queryText.AppendFormat(null, " {0}", predicateToAdd.ToQueryText(dataProvider, ref uniqueMarker));
                        this.parameters.AddRange(predicateToAdd.Parameters);
                        break;
                }
            }

            // add closing paren
            queryText.Append(" )");

            return queryText.ToString();
        }

        public IList<IDataParameter> Parameters
        {
            get { return this.parameters; }
        }

        public bool Negate { get; set; }

        public void Clear()
        {
            this.elements.Clear();
        }

        public int Count
        {
            get { return this.elements.Count; }
        }

        private IPredicateExpression AddPredicate(object elementToAdd, PredicateExpressionOperator operatorToUse)
        {
            if (elementToAdd == null)
            {
                return this;
            }

            IPredicateExpression predicate = elementToAdd as IPredicateExpression;
            if ((predicate != null) && (predicate.Count <= 0))
            {
                // empty, skip
                return this;
            }

            // the first element should not be an operator
            if (this.Count > 0)
            {
                this.elements.Add(new PredicateExpressionElement() { Type = PredicateExpressionElementType.Operator, Contents = operatorToUse });
            }

            this.elements.Add(new PredicateExpressionElement() { Type = PredicateExpressionElementType.Predicate, Contents = elementToAdd });

            return this;
        }

        private enum PredicateExpressionElementType
        {
            Predicate,
            Operator,
        }

        private class PredicateExpressionElement
        {
            public PredicateExpressionElementType Type { get; set; }
            public object Contents { get; set; }
        }
    }
}
