using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using OpenEntity.Schema;
using OpenEntity.DataProviders;

namespace OpenEntity.Query
{
    /// <summary>
    /// A predicate which encapsulates a column based constraint
    /// </summary>
    public class ColumnConstraint : IPredicate, IConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnConstraint"/> class.
        /// </summary>
        /// <param name="column">The column to constrain.</param>
        public ColumnConstraint(IColumn column)
            :this(column.Table.Name, column.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnConstraint"/> class.
        /// </summary>
        /// <param name="constraintTableName">Name of the table that the constraint column belongs to.</param>
        /// <param name="constraintColumnName">Name of the column that the constraint will be applied to.</param>
        public ColumnConstraint(string constraintTableName, string constraintColumnName)
        {
            if (string.IsNullOrEmpty(constraintTableName))
                throw new QueryGenerationException("Cannot create a constraint without a table");
            if (string.IsNullOrEmpty(constraintColumnName))
                throw new QueryGenerationException("Cannot create a constraint without a column");
            this.ColumnName = constraintColumnName;
            this.TableName = constraintTableName;

            this.Parameters = new List<IDataParameter>();
        }

        /// <summary>
        /// Gets the name of the column that the constraint will be applied to.
        /// </summary>
        public string ColumnName { get; private set; }
        /// <summary>
        /// Gets the name of the table that the column belongs to. This value may be empty if not specified.
        /// </summary>
        /// <remarks>
        /// This is really only useful in the case of a join, where the table name is required.
        /// But we still need it to obtain schema information to generate the predicate.
        /// </remarks>
        public string TableName { get; private set; }
        /// <summary>
        /// Gets the type of comparison being used for this constraint.
        /// </summary>
        internal ConstraintComparison Comparison { get; private set; }
        /// <summary>
        /// Gets a single value associated with this constraint
        /// </summary>
        public object ParameterValue { get; private set; }
        /// <summary>
        /// Gets an enumerable of values associated with this constraint.
        /// </summary>
        public IEnumerable InValues { get; private set; }
        /// <summary>
        /// For a range of values, this is the start associated with this constraint.
        /// </summary>
        public object StartValue { get; private set; }
        /// <summary>
        /// For a range of values, this is the end associated with this constraint.
        /// </summary>
        public object EndValue { get; private set; }

        /// <summary>
        /// Creates a LIKE statement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsLike(string value)
        {
            Comparison = ConstraintComparison.Like;
            ParameterValue = value;
           ///DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
        }

        /// <summary>
        /// Creates a NOT LIKE statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsNotLike(string value)
        {
            Comparison = ConstraintComparison.NotLike;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
        }

        /// <summary>
        /// Creates a "&gt;" GREATER THAN statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsGreaterThan(object value)
        {
            Comparison = ConstraintComparison.GreaterThan;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
        }

        /// <summary>
        /// Creates a "&gt;=" GREATER THAN OR EQUAL TO statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsGreaterThanOrEqualTo(object value)
        {
            Comparison = ConstraintComparison.GreaterOrEquals;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
        }

        ///// <summary>
        ///// Specifies a SQL IN statement using a nested Select statement
        ///// </summary>
        ///// <param name="selectQuery">The select query.</param>
        ///// <returns></returns>
        //public IConstraint In(Select selectQuery)
        //{
        //    //validate that there is only one column in the columnlist
        //    if (selectQuery.SelectColumnList.Length == 0 || selectQuery.SelectColumnList.Length > 1)
        //        throw new InvalidOperationException("You must specify a column to return for the IN to be valid. Use Select(\"column\") to do this");

        //    InSelect = selectQuery;

        //    Comparison = Comparison.In;
        //    return this;            
        //}

        /// <summary>
        /// Creates an IN statement
        /// </summary>
        /// <param name="values">Value array</param>
        /// <returns></returns>
        public IConstraint IsIn(IEnumerable values)
        {
            InValues = values;
            Comparison = ConstraintComparison.In;
            return this;            
        }

        ///// <summary>
        ///// Specifies a SQL IN statement using a nested Select statement
        ///// </summary>
        ///// <param name="selectQuery">The select query.</param>
        ///// <returns></returns>
        //public IConstraint NotIn(Select selectQuery)
        //{
        //    //validate that there is only one column in the columnlist
        //    if (selectQuery.SelectColumnList.Length == 0 || selectQuery.SelectColumnList.Length > 1)
        //        throw new InvalidOperationException("You must specify a column to return for the IN to be valid. Use Select(\"column\") to do this");

        //    InSelect = selectQuery;

        //    Comparison = Comparison.NotIn;
        //    return this;
            
        //}

        /// <summary>
        /// Creates a NOT IN statement
        /// </summary>
        /// <param name="values">Value array</param>
        /// <returns></returns>
        public IConstraint IsNotIn(IEnumerable values)
        {
            InValues = values;
            Comparison = ConstraintComparison.NotIn;
            return this;
            
        }

        /// <summary>
        /// Creates a "&lt;" LESS THAN statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsLessThan(object value)
        {
            Comparison = ConstraintComparison.LessThan;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
            
        }

        /// <summary>
        /// Creates a "&lt;=" LESS THAN OR EQUAL TO statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsLessThanOrEqualTo(object value)
        {
            Comparison = ConstraintComparison.LessOrEquals;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
            
        }

        /// <summary>
        /// Creates an IS NOT NULL statement
        /// </summary>
        /// <returns></returns>
        public IConstraint IsNotNull()
        {
            Comparison = ConstraintComparison.IsNot;
            ParameterValue = DBNull.Value;
            return this;
            
        }

        /// <summary>
        /// Creates an IS NULL statement
        /// </summary>
        /// <returns></returns>
        public IConstraint IsNull()
        {
            Comparison = ConstraintComparison.Is;
            ParameterValue = DBNull.Value;
            return this;
            
        }

        /// <summary>
        /// Creates an BETWEEN statement
        /// </summary>
        /// <param name="val1">The value1.</param>
        /// <param name="val2">The value2.</param>
        /// <returns></returns>
        public IConstraint IsBetweenAnd(object value1, object value2)
        {
            Comparison = ConstraintComparison.Between;
            StartValue = value1;
            EndValue = value2;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value1);
            return this;
            
        }

        /// <summary>
        /// Creates a "=" EQUALS statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsEqualTo(object value)
        {
            Comparison = ConstraintComparison.Equals;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
            
        }

        /// <summary>
        /// Creates a "!=" NOT EQUALS statement
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IConstraint IsNotEqualTo(object value)
        {
            Comparison = ConstraintComparison.NotEquals;
            ParameterValue = value;
            //DbType = query.GetConstraintDbType(TableName, ColumnName, value);
            return this;
            
        }

        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="dataProvider">The data provider containing schema information (used to build the query).</param>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker)
        {
            return this.ToQueryText(dataProvider, ref uniqueMarker, false);
        }

        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <param name="inHavingClause">if set to true, it will allow aggregate functions to be applied to colomns.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker, bool inHavingClause)
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
                queryText.Append(ColumnConstraint.GetComparisonOperator(this.Comparison));
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

        /// <summary>
        /// Gets the comparison operator string for the provided constraint comparison.
        /// </summary>
        /// <param name="comp">The constraint comparison.</param>
        /// <returns></returns>
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

        /// <summary>
        /// The list of parameters created when the Predicate was translated to text usable in a query. Only valid after a succesful call to ToQueryText
        /// </summary>
        /// <value></value>
        public IList<IDataParameter> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Flag for setting the Predicate to negate itself, i.e. to add 'NOT' to its result.
        /// </summary>
        public bool Negate
        {
            get;
            set;
        }
    }
}
