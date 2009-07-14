using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Query
{
    /// <summary>
    /// Indicates a predicate which can be ultimately parsed into a Where clause as a predicate. It contains 0-n predicates.
    /// </summary>
    public interface IPredicateExpression : IPredicate
    {
        /// <summary>
        /// Adds an IPredicate implementing object to the Predicate Expression with an 'Or'-operator. 
        /// The object added can be a Predicate derived class or a Predicate Expression . If no objects are present yet in the Predicate Expression,
        /// the operator is ignored. 
        /// </summary>
        /// <param name="predicateToAdd">The IPredicate implementing object to add</param>
        /// <exception cref="ArgumentNullException">When predicateToAdd is null</exception>
        /// <returns>the Predicate on which this method is called, for command chaining</returns>
        IPredicate AddWithOr(IPredicate predicateToAdd);
        /// <summary>
        /// Adds an IPredicate implementing object to the Predicate Expression with an 'And'-operator. 
        /// The object added can be a Predicate derived class or a PredicateExpression. If no objects are present yet in the Predicate Expression ,
        /// the operator is ignored. 
        /// </summary>
        /// <param name="predicateToAdd">The IPredicate implementing object to add</param>
        /// <exception cref="ArgumentNullException">When predicateToAdd is null</exception>
        /// <returns>the Predicate on which this method is called, for command chaining</returns>
        IPredicate AddWithAnd(IPredicate predicateToAdd);
        /// <summary>
        /// Clears the constraints and predicate expression within.
        /// </summary>
        void Clear();
        /// <summary>
        /// Gets the amount of predicate expression elements in this predicate expression. This is including all operators and constraints.
        /// </summary>
        int Count { get; }
    }
}
