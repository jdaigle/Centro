using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity
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
}
