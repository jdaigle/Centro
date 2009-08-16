using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OpenEntity.DataProviders;
using OpenEntity.Entities;
using OpenEntity.Joins;
using OpenEntity.Mapping;
using OpenEntity.Model;
using OpenEntity.Proxy;
using OpenEntity.Query;
using OpenEntity.Schema;

namespace OpenEntity.Repository
{
    public class RepositoryBase<TModelType> : IRepository<TModelType>, IRepositoryInternal, IEntityCreator where TModelType : IDomainObject
    {
        private ITable table;
        private IClassMapping classMapping;

        public RepositoryBase(IDataProvider dataProvider)
        {
            this.DataProvider = dataProvider;
            classMapping = MappingTable.FindClassMapping(typeof(TModelType));
        }

        public IDataProvider DataProvider { get; private set; }

        protected string TableName
        {
            get
            {
                if (classMapping == null)
                    throw new InvalidOperationException("Could not find class mapping for type {" + typeof(TModelType).Name + "}");
                return classMapping.Table;
            }
        }

        protected ITable Table
        {
            get
            {
                if (this.table == null)
                {
                    if (string.IsNullOrEmpty(this.TableName))
                        throw new SchemaException(string.Format(CultureInfo.InvariantCulture, "Could not determine table name for model type [{0}]", typeof(TModelType).FullName));
                    this.table = this.DataProvider.Schema.FindTable(this.TableName);
                    if (this.table == null)
                        throw new SchemaException(string.Format(CultureInfo.InvariantCulture, "Failed to find database schema for table [{0}]", this.TableName));
                }
                return this.table;
            }
            private set
            {
                this.table = value;
            }
        }

        protected IEntityFields CreateEntityFields()
        {
            IEntityField[] fieldsArray = new IEntityField[this.Table.Columns.Count];
            for (int i = 0; i < fieldsArray.Length; i++)
            {
                fieldsArray[i] = new EntityField(this.Table.Columns[i]);
            }
            EntityFieldsCollection fields = new EntityFieldsCollection(fieldsArray);
            return fields;
        }

        /// <summary>
        /// Closes the connection if it's not meant to be kept open or we're not in a transaction.
        /// </summary>
        protected internal void CloseConnectionIfPossible()
        {
            if (!this.DataProvider.KeepConnectionOpen && !this.DataProvider.IsTransactionInProgress)
            {
                this.DataProvider.CloseConnection();
            }
        }

        #region IRepository<TModelType> Members

        public virtual TModelType Create()
        {
            var modelInstance = (TModelType)EntityProxyFactory.MakeEntity(typeof(TModelType), DataProvider);
            var proxyEntity = EntityProxyFactory.AsEntity(modelInstance) as IProxyEntity;
            foreach (var property in classMapping.Properties)
                if (property.CustomTypeConverter != null)
                    proxyEntity.AddCustomTypeConverter(property.CustomTypeConverter, property.Name);
            IEntityFields fields = this.CreateEntityFields();
            if (fields == null)
                return default(TModelType);
            proxyEntity.Initialize(Table, fields);
            return modelInstance;
        }

        public virtual TModelType CreateFrom(TModelType transientObject)
        {
            if (transientObject == null)
                throw new ArgumentNullException("transientObject");
            var modelInstance = Create();
            var entity = EntityProxyFactory.AsEntity(modelInstance);
            foreach (var property in classMapping.Properties)
            {
                var field = entity.Fields[property.Column] as EntityField;
                if (field.IsReadOnly)
                    continue;
                var currentValue = property.PropertyInfo.GetValue(transientObject, null);
                field.ForceSetCurrentValue(currentValue);
                if (currentValue == null)
                    field.IsNull = true;
            }
            entity.IsDirty = false;
            entity.Fields.State = EntityState.New;
            return modelInstance;
        }

        object IEntityCreator.Create()
        {
            return Create();
        }

        public virtual bool Reload(TModelType objectToFetch)
        {
            if (objectToFetch == null)
                throw new ArgumentNullException("objectToFetch");
            if (!EntityProxyFactory.IsEntity(objectToFetch))
                throw new NotSupportedException("Cannot reload a transient model object");
            var proxyEntity = EntityProxyFactory.AsEntity(objectToFetch) as IProxyEntity;
            if (proxyEntity.IsNew)
                return false;
            IEntityFields fields = proxyEntity.Fields;
            IDbCommand selectCommand = this.CreateSelectCommand(proxyEntity.Table, proxyEntity.Table.Columns, proxyEntity.GetPrimaryKeyPredicateExpression(), null, null, 1);
            bool keepConnectionOpenSave = this.DataProvider.KeepConnectionOpen;
            try
            {
                this.DataProvider.KeepConnectionOpen = true;
                // Execute the retrieval
                this.DataProvider.ExecuteSingleRowRetrievalQuery(selectCommand, fields);
                proxyEntity.Reload();
                return true;
            }
            finally
            {
                selectCommand.Dispose();
                this.DataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
        }

        public virtual bool Save(TModelType objectToSave)
        {
            // Overload
            return this.Save(objectToSave, false);
        }

        public virtual bool Save(TModelType objectToSave, bool reloadAfterSave)
        {
            if (objectToSave == null)
                throw new ArgumentNullException("objectToSave");
            IProxyEntity proxyEntity = null;
            if (!EntityProxyFactory.IsEntity(objectToSave))
            {
                if (reloadAfterSave)
                    throw new NotSupportedException("Cannot reload a transient model object");
                var modelInstance = CreateFrom(objectToSave);
                proxyEntity = EntityProxyFactory.AsEntity(modelInstance) as IProxyEntity;
            }
            else
            {
                proxyEntity = EntityProxyFactory.AsEntity(objectToSave) as IProxyEntity;
            }

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
            bool keepConnectionOpenSave = this.DataProvider.KeepConnectionOpen;
            bool saveSucceeded = true;
            try
            {
                this.DataProvider.KeepConnectionOpen = true;

                IDbCommand saveQuery = this.CreateCommandForEntityToSave(proxyEntity.IsNew, proxyEntity, null);
                try
                {
                    //OnSaveEntity(saveQuery, entityToSave);
                    saveSucceeded = this.DataProvider.ExecuteActionQuery(saveQuery) > 0;
                    //OnSaveEntityComplete(saveQuery, entityToSave);

                    if (saveSucceeded)
                    {
                        if (proxyEntity.IsNew && proxyEntity.Fields.IdentityField != null)
                        {
                            var identityField = proxyEntity.Fields.IdentityField;
                            object identityValue = null;
                            var dummyIdentityParam = this.DataProvider.CreateOutputParameter(identityField.ColumnIndex, identityField);
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
                        if (reloadAfterSave)
                        {
                            saveSucceeded &= this.Reload(objectToSave);
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
                this.DataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return saveSucceeded;
        }

        public virtual bool Delete(TModelType objectToDelete)
        {
            if (objectToDelete == null)
                throw new ArgumentNullException("objectToDelete");
            if (!EntityProxyFactory.IsEntity(objectToDelete))
                throw new NotSupportedException("Cannot delete a transient model object");
            var proxyEntity = EntityProxyFactory.AsEntity(objectToDelete) as IProxyEntity;
            if (proxyEntity.IsNew)
            {
                // not changed or new, no fields to update, skip
                return false;
            }

            IDbCommand deleteCommand = this.CreateDeleteCommand(proxyEntity, null);

            try
            {
                //OnDeleteEntity(deleteCommand, entityToDelete);
                bool deleteSucceeded = (this.DataProvider.ExecuteActionQuery(deleteCommand) > 0);
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

        public virtual TModelType Fetch(IPredicateExpression queryPredicate)
        {
            // Overload
            return this.Fetch(queryPredicate, null);
        }

        public virtual TModelType Fetch(IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            // Overload
            return this.Fetch(queryPredicate, joinSet, null);
        }

        public virtual TModelType Fetch(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause)
        {
            TModelType fetchedEntity = default(TModelType);
            bool keepConnectionOpenSave = this.DataProvider.KeepConnectionOpen;
            try
            {
                this.DataProvider.KeepConnectionOpen = true;
                // use collection fetch for the single entity fetch, to re-use code
                var entities = this.FetchAll(queryPredicate, joinSet, orderClause, 1);
                fetchedEntity = entities.FirstOrDefault();
            }
            finally
            {
                this.DataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return fetchedEntity;
        }

        public virtual IList<TModelType> FetchAll(IPredicateExpression queryPredicate)
        {
            // Overload
            return this.FetchAll(queryPredicate, null, null, -1);
        }

        public virtual IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            // Overload
            return this.FetchAll(queryPredicate, joinSet, null, -1);
        }

        public virtual IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause)
        {
            // Overload
            return this.FetchAll(queryPredicate, joinSet, orderClause, -1);
        }

        public virtual IList<TModelType> FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause, int maxNumberOfItemsToReturn)
        {
            bool keepConnectionOpenSave = this.DataProvider.KeepConnectionOpen;
            List<TModelType> entities = new List<TModelType>();
            // Create the command
            IDbCommand selectCommand = this.CreateSelectCommand(Table, Table.Columns, queryPredicate, joinSet, orderClause, maxNumberOfItemsToReturn);
            try
            {
                this.DataProvider.KeepConnectionOpen = true;
                // Execute the retrieval
                entities = this.DataProvider.ExecuteMultiRowRetrievalQuery(selectCommand, this, false).Cast<TModelType>().ToList();
            }
            finally
            {
                selectCommand.Dispose();
                this.DataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
            return entities;
        }

        public virtual object FetchScalar(IColumn field, AggregateFunction aggregateFunction)
        {
            // Overload
            return this.FetchScalar(field, aggregateFunction, null, null);
        }

        public virtual object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate)
        {
            // Overload
            return this.FetchScalar(field, aggregateFunction, queryPredicate, null);
        }

        public virtual object FetchScalar(IColumn field, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if (aggregateFunction == AggregateFunction.None)
                return 0;
            bool keepConnectionOpenSave = this.DataProvider.KeepConnectionOpen;
            IDbCommand aggregateCommand = this.CreateAggregateCommand(Table, field, aggregateFunction, queryPredicate, joinSet);
            try
            {
                this.DataProvider.KeepConnectionOpen = true;
                return this.DataProvider.ExecuteScalarRetrievalQuery(aggregateCommand);
            }
            finally
            {
                aggregateCommand.Dispose();
                this.DataProvider.KeepConnectionOpen = keepConnectionOpenSave;
                this.CloseConnectionIfPossible();
            }
        }

        public virtual object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction)
        {
            // Overload
            return FetchScalar(columnExpression, aggregateFunction, null, null);
        }

        public virtual object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate)
        {
            // Overload
            return FetchScalar(columnExpression, aggregateFunction, queryPredicate, null);
        }
        public virtual object FetchScalar(Expression<Func<TModelType, object>> columnExpression, AggregateFunction aggregateFunction, IPredicateExpression queryPredicate, JoinSet joinSet)
        {
            var classMapping = MappingTable.FindClassMapping(typeof(TModelType));
            var columnName = classMapping.GetColumnName(columnExpression);
            var column = this.DataProvider.Schema.FindColumn(this.TableName, columnName);
            return FetchScalar(column, aggregateFunction, queryPredicate, joinSet);
        }

        #endregion

        #region IRepositoryInternal Members

        IProxyEntity IRepositoryInternal.CreateEmptyEntity()
        {
            return EntityProxyFactory.AsEntity(Create()) as IProxyEntity;
        }

        IList<object> IRepositoryInternal.FetchAll(IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause, int maxNumberOfItemsToReturn)
        {
            return FetchAll(queryPredicate, joinSet, orderClause, maxNumberOfItemsToReturn).Cast<object>().ToList();
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
            IDbCommand aggregateCommand = this.DataProvider.CreateDbCommand();
            string fieldName = this.DataProvider.QualifyColumnName(column);
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
            aggregateCommand.CommandText += Environment.NewLine + SqlFragment.FROM + this.DataProvider.QualifyTableName(fromTable);
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
                string predicateText = queryPredicate.ToQueryText(this.DataProvider, ref parameterMarker);
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
        private IDbCommand CreateSelectCommand(ITable fromTable, IList<IColumn> columnsToSelect, IPredicateExpression queryPredicate, JoinSet joinSet, IOrderClause orderClause, int maxNumberOfItemsToReturn)
        {
            IDbCommand selectCommand = this.DataProvider.CreateDbCommand();
            // generate a list of the fields to select from
            StringBuilder columns = new StringBuilder(1024);
            for (int i = 0; i < columnsToSelect.Count; i++)
            {
                if (i > 0)
                    columns.Append(", ");
                columns.Append(this.DataProvider.QualifyColumnName(columnsToSelect[i]));
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
            selectCommand.CommandText += Environment.NewLine + SqlFragment.FROM + this.DataProvider.QualifyTableName(fromTable);
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
                string predicateText = queryPredicate.ToQueryText(this.DataProvider, ref parameterMarker);
                selectCommand.CommandText += SqlFragment.WHERE + predicateText;
                foreach (IDataParameter parameter in queryPredicate.Parameters)
                    selectCommand.Parameters.Add(parameter);
            }
            if (orderClause != null)
            {
                selectCommand.CommandText += SqlFragment.ORDER_BY;
                var column = DataProvider.Schema.FindColumn(orderClause.Table, orderClause.Column);
                selectCommand.CommandText += DataProvider.QualifyColumnName(column);
                if (orderClause.Direction != SortOrder.Descending)
                    selectCommand.CommandText += SqlFragment.ASC;
                else
                    selectCommand.CommandText += SqlFragment.DESC;
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
            string joinTable = this.DataProvider.QualifyTableName(join.DestinationTable);
            var destinationColumn = this.DataProvider.Schema.FindColumn(join.DestinationTable, join.DestinationColumn);
            var originColumn = this.DataProvider.Schema.FindColumn(join.OriginTable, join.OriginColumn);
            string joinPredicate = SqlFragment.ON + this.DataProvider.QualifyColumnName(destinationColumn) + SqlFragment.EQUAL_TO + this.DataProvider.QualifyColumnName(originColumn);
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
                actionCommand = this.DataProvider.CreateDbCommand();
                StringBuilder commandText = new StringBuilder(1024);
                commandText.Append(SqlFragment.INSERT_INTO);
                commandText.AppendFormat(this.DataProvider.QualifyTableName(entityToSave.Table));
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
                        commandText.Append(this.DataProvider.QualifyColumnName(field));
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
                            IDataParameter identityOutputParameter = this.DataProvider.CreateOutputParameter(fieldIndex, field);
                            actionCommand.Parameters.Add(identityOutputParameter);
                            outputRetrievalQueryText = string.Format(";SELECT {0}={1}", identityOutputParameter.ParameterName, "SCOPE_IDENTITY()");
                        }
                        if (!field.IsIdentity && !field.IsReadOnly)
                        {
                            if (fieldIndex > 0)
                            {
                                commandText.Append(", ");
                            }
                            IDataParameter parameter = this.DataProvider.CreateParameter(fieldIndex, field, field.CurrentValue);
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
                    queryPredicate = queryPredicate.And(pkPredicateExpression);
                if (updateRestriction != null)
                    queryPredicate = queryPredicate.And(updateRestriction);
                if (queryPredicate.Count <= 0)
                {
                    // no identifying filter available. The update query will affect all rows, not only this entity. 
                    throw new DataException("The entity '" + entityToSave.Table.Name + "' doesn't have a PK defined. The update query will therefore affect all entities in the table(s), not just this entity.");
                }

                actionCommand = this.DataProvider.CreateDbCommand();
                var fieldsToUpdate = entityToSave.Fields.Where(f => f.IsChanged);
                if (fieldsToUpdate.Count() == 0)
                {
                    actionCommand.CommandText = string.Empty;
                    return actionCommand; // no fields to update return empty command
                }
                StringBuilder commandText = new StringBuilder(1024);
                // We only support single table updates, pick the table of the first field.
                commandText.AppendFormat("UPDATE {0} SET ", this.DataProvider.QualifyTableName(entityToSave.Table));
                // add the fields
                int fieldIndex = 0;
                foreach (IEntityField field in fieldsToUpdate)
                {
                    if (fieldIndex > 0)
                    {
                        commandText.Append(", ");
                    }
                    IDataParameter parameter = this.DataProvider.CreateParameter(fieldIndex, field, field.CurrentValue);
                    actionCommand.Parameters.Add(parameter);
                    commandText.AppendFormat("{0}={1}", this.DataProvider.QualifyColumnName(field), parameter.ParameterName);
                    fieldIndex++;
                }
                commandText.Append(" ");

                //append the predicate query text
                int uniqueMarker = fieldIndex;
                string queryText = queryPredicate.ToQueryText(this.DataProvider, ref uniqueMarker);
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
            IDbCommand deleteCommand = this.DataProvider.CreateDbCommand();

            IPredicateExpression pkPredicateExpression = entityToDelete.GetPrimaryKeyPredicateExpression();
            IPredicateExpression queryPredicate = new PredicateExpression();
            if (pkPredicateExpression != null)
                queryPredicate = queryPredicate.And(pkPredicateExpression);
            if (deleteRestriction != null)
                queryPredicate = queryPredicate.And(deleteRestriction);
            if (queryPredicate.Count <= 0)
            {
                // no identifying filter available. The delete query will affect all rows, not only this entity. 
                throw new DataException("The entity '" + entityToDelete.Table.Name + "' doesn't have a PK defined. The delete query will therefore delete all entities in the table(s), not just this entity.");
            }

            StringBuilder commandText = new StringBuilder(1024);
            commandText.Append(SqlFragment.DELETE_FROM);
            commandText.Append(this.DataProvider.QualifyTableName(entityToDelete.Table));

            //append the predicate query text
            int uniqueMarker = 0;
            string queryText = queryPredicate.ToQueryText(this.DataProvider, ref uniqueMarker);
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
