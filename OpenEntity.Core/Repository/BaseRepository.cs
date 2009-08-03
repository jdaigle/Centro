using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Entities;
using OpenEntity.DataProviders;
using OpenEntity.CodeDom;
using OpenEntity.Mapping;
using OpenEntity.Schema;
using System.Globalization;
using System.Data;
using OpenEntity.Query;
using OpenEntity.Joins;
using System.Diagnostics;

namespace OpenEntity.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IEntityCreator
    {
        private IDataProvider dataProvider;
        private Type entityType;
        private ITable table;
        protected string TableName { get; private set; }
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        public BaseRepository(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            this.entityType = ProxyFactory.GetProxyClass(typeof(TEntity));
            this.TableName = MappingConfig.FindClassMapping(typeof(TEntity)).Table;
        }

        public BaseRepository(IDataProvider dataProvider, string tableName)
        {
            this.dataProvider = dataProvider;
            this.TableName = tableName;
            this.entityType = typeof(EntityDataObject);
        }

        public BaseRepository(IDataProvider dataProvider, ITable table)
        {
            this.dataProvider = dataProvider;
            this.table = table;
            this.TableName = table.Name;
            this.entityType = typeof(EntityDataObject);
        }

        public void Initialize()
        {
            if (this.table == null)
            {
                if (string.IsNullOrEmpty(this.TableName))
                    throw new SchemaException(string.Format(CultureInfo.InvariantCulture, "Could not determine table name for entity type [{0}]", typeof(TEntity).FullName));
                this.table = this.dataProvider.Schema.FindTable(this.TableName);
                if (this.table == null)
                    throw new SchemaException(string.Format(CultureInfo.InvariantCulture, "Failed to find database schema for table [{0}]", this.TableName));
            }
            this.initialized = true;
        }

        public void EnsureInititalized()
        {
            if (!this.initialized)
                this.Initialize();
        }

        protected IEntityFields CreateEntityFields()
        {
            IEntityField[] fieldsArray = new IEntityField[this.table.Columns.Count];
            for (int i = 0; i < fieldsArray.Length; i++)
            {
                fieldsArray[i] = new EntityField(this.table.Columns[i]);
            }
            EntityFieldsCollection fields = new EntityFieldsCollection(fieldsArray);
            return fields;
        }

        IEntityFields IEntityCreator.CreateEntityFields()
        {
            return this.CreateEntityFields();
        }

        /// <summary>
        /// Closes the connection if it's not meant to be kept open or we're not in a transaction.
        /// </summary>
        protected internal void CloseConnectionIfPossible()
        {
            if (!this.dataProvider.KeepConnectionOpen && !this.dataProvider.IsTransactionInProgress)
            {
                this.dataProvider.CloseConnection();
            }
        }

        #region IRepository<TEntity> Members

        public TEntity Create()
        {
            this.EnsureInititalized();
            var entity = (IProxyEntity)Activator.CreateInstance(this.entityType, new object[] { this.table });
            IEntityFields fields = this.CreateEntityFields();
            if (fields == null)
                return default(TEntity);
            entity.Initialize(fields);
            return (TEntity)entity;
        }

        IProxyEntity IEntityCreator.Create()
        {
            return (IProxyEntity)this.Create();
        }

        public bool Reload(TEntity entityToFetch)
        {
            this.EnsureInititalized();
            var proxyEntity = entityToFetch as IProxyEntity;
            if (proxyEntity == null)
                throw new ArgumentNullException("entityToFetch");
            bool keepConnectionOpenSave = this.dataProvider.KeepConnectionOpen;
            if (proxyEntity.IsNew)
                return false;
            IEntityFields fields = proxyEntity.Fields;
            IDbCommand selectCommand = this.CreateSelectCommand(proxyEntity.Table, proxyEntity.Table.Columns, proxyEntity.GetPrimaryKeyPredicateExpression(), null, 1);
            try
            {
                this.dataProvider.KeepConnectionOpen = true;
                // Execute the retrieval
                this.dataProvider.ExecuteSingleRowRetrievalQuery(selectCommand, fields);
                return true;
            }
            finally
            {
                selectCommand.Dispose();
                this.dataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
        }

        public bool Save(TEntity entityToSave)
        {
            // Overload
            return this.Save(entityToSave, false);
        }

        public bool Save(TEntity entityToSave, bool refetchAfterSave)
        {
            this.EnsureInititalized();
            var proxyEntity = entityToSave as IProxyEntity;
            if (proxyEntity == null)
                throw new ArgumentNullException("entityToSave");
            if (proxyEntity.Fields.State == EntityState.Deleted)
            {
                return true; // entity to save is already deleted. Return.
            }

            if (!(proxyEntity.IsDirty || (!proxyEntity.IsDirty && proxyEntity.IsNew)))
            {
                // if the entity is not dirty, or not new      
                return true; // there is nothing to do
            }

            // process the queues
            bool keepConnectionOpenSave = this.dataProvider.KeepConnectionOpen;
            bool saveSucceeded = true;
            try
            {
                this.dataProvider.KeepConnectionOpen = true;

                IDbCommand saveQuery = this.CreateCommandForEntityToSave(proxyEntity.IsNew, proxyEntity, null);
                try
                {
                    //OnSaveEntity(saveQuery, entityToSave);
                    saveSucceeded = this.dataProvider.ExecuteActionQuery(saveQuery) > 0;
                    //OnSaveEntityComplete(saveQuery, entityToSave);

                    if (saveSucceeded)
                    {
                        if (proxyEntity.IsNew && proxyEntity.Fields.IdentityField != null)
                        {
                            var identityField = proxyEntity.Fields.IdentityField;
                            object identityValue = null;
                            var dummyIdentityParam = this.dataProvider.CreateOutputParameter(identityField.ColumnIndex, identityField);
                            foreach (IDataParameter parameter in saveQuery.Parameters)
                            {
                                if (parameter.ParameterName.Equals(dummyIdentityParam.ParameterName))
                                {
                                    identityValue = parameter.Value;
                                    break;
                                }
                            }
                            if (identityValue == DBNull.Value || identityValue == null)
                            {
                                Trace.WriteLine("Failed to obtain entity identity field value via parameter retrieval after insert statement on SaveEntity.");
                                throw new EntityOutOfSyncException("Failed to obtain entity identity field value via parameter retrieval after insert statement.");
                            }
                            (proxyEntity.Fields.IdentityField as EntityField).ForceSetCurrentValue(identityValue);
                        }
                        // set state and other housekeeping info
                        proxyEntity.Fields.State = EntityState.OutOfSync;
                        ((EntityFieldsCollection)proxyEntity.Fields).AcceptChanges();
                        proxyEntity.EndEdit();
                        proxyEntity.IsNew = false;
                        if (refetchAfterSave)
                        {
                            saveSucceeded &= this.Reload(entityToSave);
                        }
                    }
                }
                catch (CommandExecutionException ex)
                {
                    ex.EntityInvolved = proxyEntity;
                    throw;
                }
            }
            catch
            {
                saveSucceeded = false;
                throw;
            }
            finally
            {
                this.dataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return saveSucceeded;
        }

        public bool Delete(TEntity entityToDelete)
        {
            this.EnsureInititalized();
            var proxyEntity = entityToDelete as IProxyEntity;
            if (proxyEntity == null)
                throw new ArgumentNullException("entityToDelete");
            if (proxyEntity.IsNew)
            {
                // not changed or new, no fields to update, skip
                return false;
            }

            IDbCommand deleteCommand = this.CreateDeleteCommand(proxyEntity, null);

            try
            {
                //OnDeleteEntity(deleteCommand, entityToDelete);
                bool deleteSucceeded = (this.dataProvider.ExecuteActionQuery(deleteCommand) > 0);
                //OnDeleteEntityComplete(deleteCommand, entityToDelete);
                if (deleteSucceeded)
                {
                    proxyEntity.Fields.State = EntityState.Deleted;
                }
                return deleteSucceeded;
            }
            catch (CommandExecutionException ex)
            {
                ex.EntityInvolved = proxyEntity;
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                deleteCommand.Dispose();
            }
        }

        public TEntity Fetch(IPredicateExpression queryPredicate)
        {
            // Overload
            return this.Fetch(queryPredicate, null);
        }

        public TEntity Fetch(IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            this.EnsureInititalized();
            TEntity fetchedEntity = default(TEntity);
            bool keepConnectionOpenSave = this.dataProvider.KeepConnectionOpen;
            try
            {
                this.dataProvider.KeepConnectionOpen = true;
                // use collection fetch for the single entity fetch, to re-use code
                var entities = this.FetchAll(queryPredicate, joinSet, 1);
                fetchedEntity = entities.FirstOrDefault();
            }
            finally
            {
                this.dataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return fetchedEntity;
        }

        public IList<TEntity> FetchAll(IPredicateExpression queryPredicate)
        {
            // Overload
            return this.FetchAll(queryPredicate, null, -1);
        }

        public IList<TEntity> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            // Overload
            return this.FetchAll(queryPredicate, joinSet, -1);
        }

        public IList<TEntity> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, int maxNumberOfItemsToReturn)
        {
            this.EnsureInititalized();
            bool keepConnectionOpenSave = this.dataProvider.KeepConnectionOpen;
            List<TEntity> entities = new List<TEntity>();
            // Create the command
            IDbCommand selectCommand = this.CreateSelectCommand(table, table.Columns, queryPredicate, joinSet, maxNumberOfItemsToReturn);
            try
            {
                this.dataProvider.KeepConnectionOpen = true;
                // Execute the retrieval
                entities = this.dataProvider.ExecuteMultiRowRetrievalQuery(selectCommand, this, false).Cast<TEntity>().ToList();
            }
            finally
            {
                selectCommand.Dispose();
                this.dataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return entities;
        }

        public object FetchScalar(IColumn field, AggregateFunction aggregateFunction)
        {
            // Overload
            return this.FetchScalar(field, aggregateFunction, null, null);
        }

        public object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate)
        {
            // Overload
            return this.FetchScalar(field, aggregateFunction, queryPredicate, null);
        }

        public object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            this.EnsureInititalized();
            if (field == null)
                throw new ArgumentNullException("field");
            if (aggregateFunction == AggregateFunction.None)
                return 0;
            bool keepConnectionOpenSave = this.dataProvider.KeepConnectionOpen;
            IDbCommand aggregateCommand = this.CreateAggregateCommand(table, field, aggregateFunction, queryPredicate, joinSet);
            try
            {
                this.dataProvider.KeepConnectionOpen = true;
                return this.dataProvider.ExecuteScalarRetrievalQuery(aggregateCommand);
            }
            finally
            {
                aggregateCommand.Dispose();
                this.dataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
        }

        #endregion

        #region Command Creation

        /// <summary>
        /// Creates the aggregate command for the given table, field, function and query predicate
        /// </summary>
        /// <param name="fromTable">From table.</param>
        /// <param name="column">The column.</param>
        /// <param name="aggregateFunction">The aggregate function.</param>
        /// <param name="queryPredicate">The query predicate.</param>
        /// <param name="joinSet">The joins to walk.</param>
        /// <returns></returns>
        private IDbCommand CreateAggregateCommand(ITable fromTable, IColumn column, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            IDbCommand aggregateCommand = this.dataProvider.CreateDbCommand();
            string fieldName = this.dataProvider.QualifyColumnName(column);
            string selectStatement = SqlFragment.SELECT;
            switch (aggregateFunction)
            {
                case AggregateFunction.Avg:
                    selectStatement += AggregateFunctionName.AVERAGE + " ( " + fieldName + " ) ";
                    break;
                case AggregateFunction.AvgDistinct:
                    selectStatement += AggregateFunctionName.AVERAGE + " ( " + SqlFragment.DISTINCT + fieldName + " ) ";
                    break;
                case AggregateFunction.Count:
                    selectStatement += AggregateFunctionName.COUNT + " ( " + fieldName + " ) ";
                    break;
                case AggregateFunction.CountDistinct:
                    selectStatement += AggregateFunctionName.COUNT + " ( " + SqlFragment.DISTINCT + fieldName + " ) ";
                    break;
                case AggregateFunction.CountRow:
                    selectStatement += AggregateFunctionName.COUNT + " ( * ) ";
                    break;
                case AggregateFunction.Max:
                    selectStatement += AggregateFunctionName.MAX + " ( " + fieldName + " ) ";
                    break;
                case AggregateFunction.Min:
                    selectStatement += AggregateFunctionName.MIN + " ( " + fieldName + " ) ";
                    break;
                case AggregateFunction.Sum:
                    selectStatement += AggregateFunctionName.SUM + " ( " + fieldName + " ) ";
                    break;
                case AggregateFunction.SumDistinct:
                    selectStatement += AggregateFunctionName.SUM + " ( " + SqlFragment.DISTINCT + fieldName + " ) ";
                    break;
            }
            aggregateCommand.CommandText = selectStatement;
            aggregateCommand.CommandText += Environment.NewLine + SqlFragment.FROM + this.dataProvider.QualifyTableName(fromTable);
            aggregateCommand.CommandText += Environment.NewLine;
            // walk the relationships and add the joins if necessary
            if (joinSet != null && joinSet.Count() > 0)
            {
                aggregateCommand.CommandText += this.WalkJoins(joinSet);
            }
            // add the predicate
            if (queryPredicate != null && queryPredicate.Count > 0)
            {
                int parameterMarker = 0;
                string predicateText = queryPredicate.ToQueryText(this.dataProvider, ref parameterMarker);
                aggregateCommand.CommandText += SqlFragment.WHERE + predicateText;
                foreach (IDataParameter parameter in queryPredicate.Parameters)
                    aggregateCommand.Parameters.Add(parameter);
            }
            return aggregateCommand;
        }

        /// <summary>
        /// Creates a select command for a given table and the provided query predicate.
        /// </summary>
        /// <param name="fromTable">From table.</param>
        /// <param name="columnsToSelect">The columns to select.</param>
        /// <param name="queryPredicate">The query predicate.</param>
        /// <param name="joinsToWalk">The joins to walk.</param>
        /// <returns></returns>
        private IDbCommand CreateSelectCommand(ITable fromTable, IList<IColumn> columnsToSelect, IPredicateExpression queryPredicate, JoinSet joinSet, int maxNumberOfItemsToReturn)
        {
            IDbCommand selectCommand = this.dataProvider.CreateDbCommand();
            // generate a list of the fields to select from
            StringBuilder columns = new StringBuilder(1024);
            for (int i = 0; i < columnsToSelect.Count; i++)
            {
                if (i > 0)
                    columns.Append(", ");
                columns.Append(this.dataProvider.QualifyColumnName(columnsToSelect[i]));
            }
            string selectStatement = SqlFragment.SELECT;
            if (joinSet != null && joinSet.Count() > 0)
                selectStatement += SqlFragment.DISTINCT;
#warning this will probably break for Oracle, need to test this
            if (maxNumberOfItemsToReturn > 0)
            {
                selectStatement += SqlFragment.TOP;
                selectStatement += maxNumberOfItemsToReturn.ToString();
            }
            selectCommand.CommandText = selectStatement + columns.ToString();
            selectCommand.CommandText += Environment.NewLine + SqlFragment.FROM + this.dataProvider.QualifyTableName(fromTable);
            selectCommand.CommandText += Environment.NewLine;
            // walk the relationships and add the joins if necessary
            if (joinSet != null && joinSet.Count() > 0)
            {
                selectCommand.CommandText += this.WalkJoins(joinSet);
            }
            // add the predicate
            if (queryPredicate != null && queryPredicate.Count > 0)
            {
                int parameterMarker = 0;
                string predicateText = queryPredicate.ToQueryText(this.dataProvider, ref parameterMarker);
                selectCommand.CommandText += SqlFragment.WHERE + predicateText;
                foreach (IDataParameter parameter in queryPredicate.Parameters)
                    selectCommand.Parameters.Add(parameter);
            }
            return selectCommand;
        }

        /// <summary>
        /// Walks the relationships and produces the aggregate JOIN statements
        /// </summary>
        /// <param name="joins">The join collection.</param>
        private string WalkJoins(JoinSet joins)
        {
            string joinText = string.Empty;
            foreach (var join in joins)
            {
                joinText += this.CreateJoin(join);
                joinText += Environment.NewLine;
                // recursively get the relationships
                if (join.JoinSet.Count() > 0)
                {
                    joinText += this.WalkJoins(join.JoinSet);
                }
            }
            return joinText;
        }

        /// <summary>
        /// Creates a JOIN statement for for the given relationship
        /// </summary>
        /// <param name="join">The join.</param>
        private string CreateJoin(Join join)
        {
            string joinType = string.Empty;
            switch (join.RelationshipType)
            {
                case RelationshipType.OneToMany:
                    joinType = SqlFragment.LEFT_OUTER_JOIN;
                    break;
                case RelationshipType.OneToOne:
                    joinType = SqlFragment.INNER_JOIN;
                    break;
            }
            string joinTable = this.dataProvider.QualifyTableName(join.DestinationTable);
            var destinationColumn = this.dataProvider.Schema.FindColumn(join.DestinationTable, join.DestinationColumn);
            var originColumn = this.dataProvider.Schema.FindColumn(join.OriginTable, join.OriginColumn);
            string joinPredicate = SqlFragment.ON + this.dataProvider.QualifyColumnName(destinationColumn) + SqlFragment.EQUAL_TO + this.dataProvider.QualifyColumnName(originColumn);
            return joinType + joinTable + joinPredicate;
        }

        ///// <summary>
        ///// Creates a select command for a given table and the provided query predicate.
        ///// </summary>
        //protected abstract IRetrievalCommand CreateSelectCommand(IEntityFields2 fieldsToFetch, IFieldPersistenceInfo[] persistenceInfoObjects,
        //    IPredicateExpression filter, long maxNumberOfItemsToReturn, ISortExpression sortClauses, IRelationCollection relationsToWalk,
        //    bool allowDuplicates, IGroupByCollection groupByClause, int pageNumber, int pageSize);

        /// <summary>
        /// Creates an action command to insert or update an entity's fields.
        /// </summary>
        /// <param name="insertActions">if set to <c>true</c> then it is an insert command. Otherwise it is an update.</param>
        /// <param name="entityToSave">The entity to save.</param>
        /// <param name="updateRestriction">The update restriction predicate to use.</param>
        /// <returns></returns>
        private IDbCommand CreateCommandForEntityToSave(bool insertActions, IProxyEntity entityToSave, IPredicateExpression updateRestriction)
        {
            IDbCommand actionCommand = null;
            if (insertActions)
            {
                var outputRetrievalQueryText = string.Empty;
                actionCommand = this.dataProvider.CreateDbCommand();
                StringBuilder commandText = new StringBuilder(1024);
                commandText.Append(SqlFragment.INSERT_INTO);
                commandText.AppendFormat(this.dataProvider.QualifyTableName(entityToSave.Table));
                // indicate the fields we will be inserting.
                commandText.Append(" ( ");
                int fieldIndex = 0;
                foreach (IEntityField field in entityToSave.Fields)
                {
                    if (!field.IsIdentity && !field.IsReadOnly)
                    {
                        if (fieldIndex > 0)
                        {
                            commandText.Append(", ");
                        }
                        commandText.Append(this.dataProvider.QualifyColumnName(field));
                        fieldIndex++;
                    }
                }
                commandText.Append(" ) ");
                // parameterize the fields we will be inserting
                if (fieldIndex > 0)
                {
                    commandText.Append("VALUES (");
                    fieldIndex = 0;
                    foreach (IEntityField field in entityToSave.Fields)
                    {
                        if (field.IsIdentity)
                        {
                            IDataParameter identityOutputParameter = this.dataProvider.CreateOutputParameter(fieldIndex, field);
                            actionCommand.Parameters.Add(identityOutputParameter);
                            outputRetrievalQueryText = string.Format(";SELECT {0}={1}", identityOutputParameter.ParameterName, "SCOPE_IDENTITY()");
                        }
                        if (!field.IsIdentity && !field.IsReadOnly)
                        {
                            if (fieldIndex > 0)
                            {
                                commandText.Append(", ");
                            }
                            IDataParameter parameter = this.dataProvider.CreateParameter(fieldIndex, field, field.CurrentValue);
                            actionCommand.Parameters.Add(parameter);
                            commandText.AppendFormat("{0}", parameter.ParameterName);
                            fieldIndex++;
                        }
                    }
                    commandText.Append(" )");
                }
                commandText.Append(outputRetrievalQueryText);
                commandText.Append(";");
                // set the command text
                actionCommand.CommandText = commandText.ToString();
            }
            else
            {
                IPredicateExpression pkPredicateExpression = entityToSave.GetPrimaryKeyPredicateExpression();
                IPredicateExpression queryPredicate = new PredicateExpression();
                if (pkPredicateExpression != null)
                    queryPredicate.AddWithAnd(pkPredicateExpression);
                if (updateRestriction != null)
                    queryPredicate.AddWithAnd(updateRestriction);
                if (queryPredicate.Count <= 0)
                {
                    // no identifying filter available. The update query will affect all rows, not only this entity. 
                    throw new DataException("The entity '" + entityToSave.Table.Name + "' doesn't have a PK defined. The update query will therefore affect all entities in the table(s), not just this entity.");
                }

                actionCommand = this.dataProvider.CreateDbCommand();
                var fieldsToUpdate = entityToSave.Fields.Where(f => f.IsChanged);
                if (fieldsToUpdate.Count() == 0)
                {
                    actionCommand.CommandText = string.Empty;
                    return actionCommand; // no fields to update return empty command
                }
                StringBuilder commandText = new StringBuilder(1024);
                // We only support single table updates, pick the table of the first field.
                commandText.AppendFormat("UPDATE {0} SET ", this.dataProvider.QualifyTableName(entityToSave.Table));
                // add the fields
                int fieldIndex = 0;
                foreach (IEntityField field in fieldsToUpdate)
                {
                    if (fieldIndex > 0)
                    {
                        commandText.Append(", ");
                    }
                    IDataParameter parameter = this.dataProvider.CreateParameter(fieldIndex, field, field.CurrentValue);
                    actionCommand.Parameters.Add(parameter);
                    commandText.AppendFormat("{0}={1}", this.dataProvider.QualifyColumnName(field), parameter.ParameterName);
                    fieldIndex++;
                }
                commandText.Append(" ");

                //append the predicate query text
                int uniqueMarker = fieldIndex;
                string queryText = queryPredicate.ToQueryText(this.dataProvider, ref uniqueMarker);
                commandText.Append(SqlFragment.WHERE);
                commandText.Append(queryText);
                foreach (IDataParameter parameter in queryPredicate.Parameters)
                    actionCommand.Parameters.Add(parameter);

                // set the command text
                actionCommand.CommandText = commandText.ToString();
            }
            return actionCommand;
        }

        /// <summary>
        /// Creates a new Delete Command object which is ready to use.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        /// <param name="deleteRestriction">The delete restriction.</param>
        /// <returns>
        /// IActionCommand instance which is ready to be used.
        /// </returns>
        private IDbCommand CreateDeleteCommand(IProxyEntity entityToDelete, IPredicateExpression deleteRestriction)
        {
            IDbCommand deleteCommand = this.dataProvider.CreateDbCommand();

            IPredicateExpression pkPredicateExpression = entityToDelete.GetPrimaryKeyPredicateExpression();
            IPredicateExpression queryPredicate = new PredicateExpression();
            if (pkPredicateExpression != null)
                queryPredicate.AddWithAnd(pkPredicateExpression);
            if (deleteRestriction != null)
                queryPredicate.AddWithAnd(deleteRestriction);
            if (queryPredicate.Count <= 0)
            {
                // no identifying filter available. The delete query will affect all rows, not only this entity. 
                throw new DataException("The entity '" + entityToDelete.Table.Name + "' doesn't have a PK defined. The delete query will therefore delete all entities in the table(s), not just this entity.");
            }

            StringBuilder commandText = new StringBuilder(1024);
            commandText.Append(SqlFragment.DELETE_FROM);
            commandText.Append(this.dataProvider.QualifyTableName(entityToDelete.Table));

            //append the predicate query text
            int uniqueMarker = 0;
            string queryText = queryPredicate.ToQueryText(this.dataProvider, ref uniqueMarker);
            commandText.Append(SqlFragment.WHERE);
            commandText.Append(queryText);
            foreach (IDataParameter parameter in queryPredicate.Parameters)
                deleteCommand.Parameters.Add(parameter);

            deleteCommand.CommandText = commandText.ToString();

            return deleteCommand;
        }

        #endregion
    }
}
