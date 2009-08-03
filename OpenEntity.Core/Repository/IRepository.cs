using System.Collections.Generic;
using OpenEntity.Joins;
using OpenEntity.Model;
using OpenEntity.Query;
using OpenEntity.Schema;
using System.Linq.Expressions;
using System;

namespace OpenEntity.Repository
{
    public interface IRepository<TModelType> where TModelType : IDomainObject
    {
        TModelType Create();
        bool Reload(TModelType objectToFetch);
        bool Save(TModelType objectToSave);
        bool Save(TModelType objectToSave, bool reloadAfterSave);
        bool Delete(TModelType objectToDelete);
        TModelType Fetch(IPredicateExpression queryPredicate);
        TModelType Fetch(IPredicateExpression queryPredicate, JoinSet joinSet);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, int maxNumberOfItemsToReturn);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet);
    }
}
