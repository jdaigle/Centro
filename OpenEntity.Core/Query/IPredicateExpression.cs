using System;
using System.Linq.Expressions;
using OpenEntity.Model;

namespace OpenEntity.Query
{
    /// <summary>
    /// Indicates a predicate which can be ultimately parsed into a Where clause as a predicate. It contains 0-n predicates.
    /// </summary>
    public interface IPredicateExpression : IPredicate
    {
        IPredicateExpression And(IPredicate predicate);
        IPredicateExpression Or(IPredicate predicate);
        IConstraint Where<TModelType>(Expression<Func<TModelType, object>> columnExpression) where TModelType : IDomainObject;
        IConstraint And<TModelType>(Expression<Func<TModelType, object>> columnExpression) where TModelType : IDomainObject;
        IConstraint Or<TModelType>(Expression<Func<TModelType, object>> columnExpression) where TModelType : IDomainObject;
    }
}
