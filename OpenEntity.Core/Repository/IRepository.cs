using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Entities;
using OpenEntity.Query;
using OpenEntity.Schema;
using OpenEntity.Joins;

namespace OpenEntity.Repository
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Creates an instance of the typed entity. It will be populated with information from the schema this data provider is related to.
        /// <returns></returns>
        TEntity Create();
        /// <summary>
        /// Fetches an entity from the database into the passed in IEntity object using a primary key filter. The primary key fields of
        /// the entity passed in have to have the primary key values. (Example: CustomerID has to have a value, when you want to fetch a CustomerEntity
        /// from the persistent storage into the passed in object)
        /// </summary>
        /// <param name="entityToFetch">The entity object in which the fetched entity data will be stored. The primary key fields have to have a value.</param>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other concurrency scheme used by the database can be
        /// utilized
        /// </remarks>
        /// <returns>true if the Fetch was succesful, false otherwise</returns>
        bool Reload(TEntity entityToFetch);
        /// <summary>
        /// Saves the passed in entity to the database. Will <i>not</i> refetch the entity after this save.
        /// The entity will stay out-of-sync. If the entity is new, it will be inserted, if the entity is existent, the changed
        /// entity fields will be changed in the database.
        /// </summary>
        /// <param name="entityToSave">The entity to save</param>
        /// <returns>true if the save was succesful, false otherwise.</returns>
        /// <remarks>Will use a current transaction if a transaction is in progress</remarks>
        bool Save(TEntity entityToSave);
        /// <summary>
        /// Saves the passed in entity to the database. If the entity is new, it will be inserted, if the entity is existent, the changed
        /// entity fields will be changed in the database.
        /// </summary>
        /// <param name="entityToSave">The entity to save</param>
        /// <param name="refetchAfterSave">When true, it will refetch the entity from the persistent storage so it will be up-to-date
        /// after the save action.</param>
        /// <returns>true if the save was succesful, false otherwise.</returns>
        /// <remarks>Will use a current transaction if a transaction is in progress</remarks>
        bool Save(TEntity entityToSave, bool refetchAfterSave);
        /// <summary>
        /// Deletes the specified entity from the database. The entity is not usable after this call, the state is set to OutOfSync.
        /// Will use the current transaction if a transaction is in progress.
        /// </summary>
        /// <param name="entityToDelete">The entity instance to delete from the database</param>
        /// <returns>true if the delete was succesful, otherwise false.</returns>
        bool Delete(TEntity entityToDelete);
        /// <summary>
        /// Fetches a new entity using the query filter passed in.
        /// </summary>
        /// <param name="queryPredicate">The constraint to use when fetching the entity.</param>
        /// <returns>The new entity fetched.</returns>
        TEntity Fetch(IPredicateExpression queryPredicate);
        /// <summary>
        /// Fetches a new entity using the query filter passed in.
        /// </summary>
        /// <param name="queryPredicate">The constraint to use when fetching the entity.</param>
        /// <param name="joinSet">A set of joins used to join this entity to other entities. Can be null.</param>
        /// <returns>The new entity fetched.</returns>
        TEntity Fetch(IPredicateExpression queryPredicate, JoinSet joinSet);
        /// <summary>
        /// Fetches one or more entities which match the filter information in the query predicate into a typed collection.
        /// This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="queryPredicate">Filter information for retrieving the entities. If null, all entities are returned of the type specified.</param>
        IList<TEntity> FetchAll(IPredicateExpression queryPredicate);
        /// <summary>   
        /// Fetches one or more entities which match the filter information in the query predicate into a typed Collection.
        /// This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="queryPredicate">Filter information for retrieving the entities. If null, all entities are returned of the type specified.</param>
        /// <param name="joinSet">A set of joins used to join this entity to other entities. Can be null.</param>
        /// <returns></returns>
        IList<TEntity> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet);
        /// <summary>
        /// Fetches one or more entities which match the filter information in the query predicate into a typed Collection.
        /// This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="queryPredicate">Filter information for retrieving the entities. If null, all entities are returned of the type specified.</param>
        /// <param name="joinSet">A set of joins used to join this entity to other entities. Can be null.</param>
        /// <param name="maxNumberOfItemsToReturn">A limit to the number of items to return from the repository.</param>
        /// <returns></returns>
        IList<TEntity> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, int maxNumberOfItemsToReturn);
        /// <summary>
        /// Gets a scalar value, calculated with the aggregate specified. The field specified is the field the aggregate is
        /// applied on.
        /// </summary>
        /// <param name="field">The field the aggregate is applied on.</param>
        /// <param name="aggregateFunction">The aggregate function to calculate.</param>
        /// <returns>
        /// The numeric result of the aggregate function.
        /// </returns>
        object FetchScalar(IColumn field, AggregateFunction aggregateFunction);
        /// <summary>
        /// Gets a scalar value, calculated with the aggregate specified. The field specified is the field the aggregate is
        /// applied on.
        /// </summary>
        /// <param name="field">The field the aggregate is applied on.</param>
        /// <param name="aggregateFunction">The aggregate function to calculate.</param>
        /// <param name="queryPredicate">The query predicate to apply to retrieve the scalar.</param>
        /// <returns>
        /// The numeric result of the aggregate function.
        /// </returns>
        object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate);
        /// <summary>
        /// Gets a scalar value, calculated with the aggregate specified. The field specified is the field the aggregate is
        /// applied on.
        /// </summary>
        /// <param name="field">The field the aggregate is applied on.</param>
        /// <param name="aggregateFunction">The aggregate function to calculate.</param>
        /// <param name="queryPredicate">The query predicate to apply to retrieve the scalar.</param>
        /// <param name="joinSet">A set of joins used to join this entity to other entities. Can be null.</param>
        /// <returns>
        /// The numeric result of the aggregate function.
        /// </returns>
        object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet);
    }
}
