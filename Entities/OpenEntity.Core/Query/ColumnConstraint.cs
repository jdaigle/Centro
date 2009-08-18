using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Schema;

namespace Centro.OpenEntity.Query
{
    internal class ColumnConstraint : IPredicate, IConstraint
    {
        internal ColumnConstraint(IColumn column, IPredicateExpression predicateExpression, PredicateExpressionOperator operatorToUse)
            : this(column.Table.Name, column.Name, predicateExpression, operatorToUse)
        {
        }

        internal ColumnConstraint(string constraintTableName, string constraintColumnName, IPredicateExpression predicateExpression, PredicateExpressionOperator operatorToUse)
        {
            this.predicateExpression = predicateExpression;
            if (string.IsNullOrEmpty(constraintTableName))
                throw new QueryGenerationException("Cannot create a constraint without a table");
            if (string.IsNullOrEmpty(constraintColumnName))
                throw new QueryGenerationException("Cannot create a constraint without a column");
            this.ColumnName = constraintColumnName;
            this.TableName = constraintTableName;
            this.Parameters = new List<IDataParameter>();
            if (operatorToUse == PredicateExpressionOperator.And)
                predicateExpression.And(this);
            else
                predicateExpression.Or(this);
        }

        private IPredicateExpression predicateExpression;

        public string ColumnName { get; private set; }
        public string TableName { get; private set; }
        private ConstraintComparison Comparison { get; set; }
        public object ParameterValue { get; private set; }
        public IEnumerable InValues { get; private set; }
        public object StartValue { get; private set; }
        public object EndValue { get; private set; }

        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataRepository", "Cannot create query part without a schema from the data repository.");
            }

            IColumn column = dataProvider.Schema.FindColumn(this.TableName, this.ColumnName);
            if (column == null)
                throw new QueryGenerationException("Could not find column schema information for the constraint: " + this.TableName + "." + this.ColumnName);

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

            if (this.Comparison == ConstraintComparison.Between)
            {
                if (this.StartValue == null || this.EndValue == null)
                    throw new QueryGenerationException("Start or End value for a BETWEEN constraint is null");

                // need to create the parameters
                IDataParameter start = dataProvider.CreateParameter(uniqueMarker, column, this.StartValue);
                uniqueMarker++; //always increment after using
                this.Parameters.Add(start);
                IDataParameter end = dataProvider.CreateParameter(uniqueMarker, column, this.EndValue);
                uniqueMarker++; //always increment after using
                this.Parameters.Add(end);

                queryText.Append(dataProvider.QualifyColumnName(column));
                queryText.Append(SqlFragment.BETWEEN);
                queryText.Append(start.ParameterName);
                queryText.Append(SqlFragment.AND);
                queryText.Append(end.ParameterName);
            }
            else if (this.Comparison == ConstraintComparison.In || this.Comparison == ConstraintComparison.NotIn)
            {

            }
            else
            {
                queryText.Append(dataProvider.QualifyColumnName(column));
                queryText.Append(GetComparisonOperator(this.Comparison));
                if (this.Comparison == ConstraintComparison.Is || this.Comparison == ConstraintComparison.IsNot)
                {
                    if (this.ParameterValue == null || this.ParameterValue == DBNull.Value)
                    {
                        queryText.Append("NULL");
                    }
                }
                else
                {
                    IDataParameter value = dataProvider.CreateParameter(uniqueMarker, column, this.ParameterValue);
                    uniqueMarker++; //always increment after using
                    this.Parameters.Add(value);
                    queryText.Append(value.ParameterName);
                }
            }

            // add ending paren
            queryText.Append(")");
            return queryText.ToString();
        }

        internal static string GetComparisonOperator(ConstraintComparison comp)
        {
            string sOut;
            switch (comp)
            {
                case ConstraintComparison.GreaterThan:
                    sOut = SqlComparison.GREATER;
                    break;
                case ConstraintComparison.GreaterOrEquals:
                    sOut = SqlComparison.GREATER_OR_EQUAL;
                    break;
                case ConstraintComparison.LessThan:
                    sOut = SqlComparison.LESS;
                    break;
                case ConstraintComparison.LessOrEquals:
                    sOut = SqlComparison.LESS_OR_EQUAL;
                    break;
                case ConstraintComparison.Like:
                    sOut = SqlComparison.LIKE;
                    break;
                case ConstraintComparison.NotEquals:
                    sOut = SqlComparison.NOT_EQUAL;
                    break;
                case ConstraintComparison.NotLike:
                    sOut = SqlComparison.NOT_LIKE;
                    break;
                case ConstraintComparison.Is:
                    sOut = SqlComparison.IS;
                    break;
                case ConstraintComparison.IsNot:
                    sOut = SqlComparison.IS_NOT;
                    break;
                case ConstraintComparison.In:
                    sOut = " IN ";
                    break;
                case ConstraintComparison.NotIn:
                    sOut = " NOT IN ";
                    break;
                default:
                    sOut = SqlComparison.EQUAL;
                    break;
            }
            return sOut;
        }

        public IList<IDataParameter> Parameters
        {
            get;
            private set;
        }

        public bool Negate
        {
            get;
            set;
        }

        public void Clear()
        {
            // no op
        }

        public int Count
        {
            get { return 1; }
        }

        public IPredicateExpression IsLike(string value)
        {
            Comparison = ConstraintComparison.Like;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsNotLike(string value)
        {
            Comparison = ConstraintComparison.NotLike;
            ParameterValue = value;

            return predicateExpression;
        }

        public IPredicateExpression IsGreaterThan(object value)
        {
            Comparison = ConstraintComparison.GreaterThan;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsGreaterThanOrEqualTo(object value)
        {
            Comparison = ConstraintComparison.GreaterOrEquals;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsIn(IEnumerable values)
        {
            InValues = values;
            Comparison = ConstraintComparison.In;
            return predicateExpression;
        }

        public IPredicateExpression IsNotIn(IEnumerable values)
        {
            InValues = values;
            Comparison = ConstraintComparison.NotIn;
            return predicateExpression;
        }

        public IPredicateExpression IsLessThan(object value)
        {
            Comparison = ConstraintComparison.LessThan;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsLessThanOrEqualTo(object value)
        {
            Comparison = ConstraintComparison.LessOrEquals;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsNotNull()
        {
            Comparison = ConstraintComparison.IsNot;
            ParameterValue = DBNull.Value;
            return predicateExpression;
        }

        public IPredicateExpression IsNull()
        {
            Comparison = ConstraintComparison.Is;
            ParameterValue = DBNull.Value;
            return predicateExpression;
        }

        public IPredicateExpression IsBetweenAnd(object value1, object value2)
        {
            Comparison = ConstraintComparison.Between;
            StartValue = value1;
            EndValue = value2;
            return predicateExpression;
        }

        public IPredicateExpression IsEqualTo(object value)
        {
            Comparison = ConstraintComparison.Equals;
            ParameterValue = value;
            return predicateExpression;
        }

        public IPredicateExpression IsNotEqualTo(object value)
        {
            Comparison = ConstraintComparison.NotEquals;
            ParameterValue = value;
            return predicateExpression;
        }
    }
}
