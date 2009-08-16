using System.Collections.Generic;
using OpenEntity.Joins;
using OpenEntity.Model;
using OpenEntity.Query;
using OpenEntity.Schema;
using System.Linq.Expressions;
using System;
using OpenEntity.Entities;

namespace OpenEntity.Repository
{
    internal interface IRepositoryInternal
    {
        IProxyEntity CreateEmptyEntity();
        IList<object> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause, int maxNumberOfItemsToReturn);        
    }

    public interface IRepository<TModelType> where TModelType : IDomainObject
    {        
        TModelType Create();
        TModelType CreateFrom(TModelType transientObject);
        bool Reload(TModelType objectToFetch);
        bool Save(TModelType objectToSave);
        bool Save(TModelType objectToSave, bool reloadAfterSave);
        bool Delete(TModelType objectToDelete);
        TModelType Fetch(IPredicateExpression queryPredicate);
        TModelType Fetch(IPredicateExpression queryPredicate, JoinSet joinSet);
        TModelType Fetch(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause);
        IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause, int maxNumberOfItemsToReturn);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate);
        object FetchScalar(IColumn column, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate);
        object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet);
    }
}
