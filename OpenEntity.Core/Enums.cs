using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity
{
    /// <summary>
    /// SQL Constraint Comparison Operators
    /// </summary>
    internal enum ConstraintComparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        Is,
        IsNot,
        In,
        NotIn,
        Between,
    }
   
    /// <summary>
    /// Enum definition for the Operators used in PredicateExpressions
    /// </summary>
    internal enum PredicateExpressionOperator : int
    {
        /// <summary>
        /// The AND operator.
        /// </summary>
        And,
        /// <summary>
        /// The OR operator.
        /// </summary>
        Or
    }

    /// <summary>
    /// Enum definition for the type of a predicate instance.
    /// This enum is stored in Predicate as an int and is used in DataAccessAdapterBase derived classes.
    /// </summary>
    internal enum PredicateType : int
    {
        Undefined = 0,
        Between,
        Empty,
        Equals,
        Like,
        PredicateExpression,
        In,
        FieldFullTextSearchPredicate,
        FieldCompareExpressionPredicate,
        FieldCompareSetPredicate
    }
}
