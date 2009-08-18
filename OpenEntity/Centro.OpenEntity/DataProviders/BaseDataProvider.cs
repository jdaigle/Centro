using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Centro.OpenEntity.Schema;
using System.IO;
using System.Data.Common;
using System.Reflection;
using Centro.OpenEntity.Entities;
using System.Diagnostics;
using Centro.OpenEntity.Proxy;

namespace Centro.OpenEntity.DataProviders
{
    public abstract class BaseDataProvider : IDataProvider, ISchemaProvider
    {
        protected BaseDataProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.Schema = new EmptySchema();
            this.Schema.SetSchemaProvider(this);
            this.CommandTimeout = 30;
        }

        #region Class Instance Fields
        private IDbConnection activeConnection;
        private IDbTransaction activeTransaction;
        private HashSet<string> savePointNames = new HashSet<string>();
        private IList<TextWriter> logListeners = new List<TextWriter>();
        #endregion

        #region Data Provider Abstract Methods
        /// <summary>
        /// Creates a new physical connection object.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// IDbConnection implementing connection object.
        /// </returns>
        protected abstract IDbConnection CreateNewConnection(string connectionString);

        /// <summary>
        /// Creates a new physical transaction object over the created connection. The connection is assumed to be open.
        /// </summary>
        /// <returns>a physical transaction object, like an instance of SqlTransaction.</returns>
        protected abstract IDbTransaction CreateNewTransaction();

        /// <summary>
        /// Creates a new .NET DataAdapter for the database system this DataRepository object is targeting. 
        /// </summary>
        /// <returns>New .NET DataAdapter object</returns>
        protected abstract DbDataAdapter CreateNewDataAdapter();

        /// <summary>
        /// Creates a new client specific IDbCommand object.
        /// </summary>
        public abstract IDbCommand CreateDbCommand();

        /// <summary>
        /// Gets the the name of the default schema for this data provider.
        /// </summary>
        protected abstract string GetDefaultSchemaName();
        #endregion

        /// <summary>
        /// Creates a new connection object using the current connection string value
        /// </summary>
        /// <remarks>Will close and dispose an active connection.</remarks>
        protected void CreateConnection()
        {
            if (this.activeConnection != null)
            {
                if (this.activeConnection.State == ConnectionState.Open)
                {
                    this.CloseConnection();
                }
            }

            this.activeConnection = this.CreateNewConnection(this.ConnectionString);
        }

        /// <summary>
        /// Returns the active connection object. If no connection object is present, a new one will be created.
        /// </summary>
        /// <returns>The active connection object</returns>
        protected internal IDbConnection GetActiveConnection()
        {
            if (this.activeConnection == null)
            {
                this.activeConnection = this.CreateNewConnection(this.ConnectionString);
            }

            return this.activeConnection;
        }

        public void OpenConnection()
        {
            if (this.activeConnection == null)
            {
                this.CreateConnection();
            }

            if (this.activeConnection.State != ConnectionState.Open)
            {
                this.activeConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (this.activeConnection != null)
            {
                if (this.activeConnection.State != ConnectionState.Closed)
                {
                    this.activeConnection.Close();
                    this.activeConnection = null;
                    this.KeepConnectionOpen = false;
                }
            }
        }

        public int CommandTimeout { get; set; }

        public string ConnectionString { get; private set; }

        public bool KeepConnectionOpen { get; set; }

        /// <summary>
        /// Closes the connection if it's not meant to be kept open or we're not in a transaction.
        /// </summary>
        protected internal void CloseConnectionIfPossible()
        {
            if (!this.KeepConnectionOpen && !this.IsTransactionInProgress)
            {
                this.CloseConnection();
            }
        }

        public void StartTransaction(string name)
        {
            this.StartTransaction(IsolationLevel.Unspecified, name);
        }

        public void StartTransaction(IsolationLevel isolationLevelToUse, string name)
        {
            if (this.IsTransactionInProgress)
            {
                throw new InvalidOperationException(String.Format("A transaction with the name '{0}' is already in progress. Commit or rollback that transaction first.", this.TransactionName));
            }
            this.TransactionName = name;
            this.TransactionIsolationLevel = isolationLevelToUse;
            this.OpenConnection();
            this.activeTransaction = this.CreateNewTransaction();
            if (this.activeTransaction != null)
                this.IsTransactionInProgress = true;
        }

        public void SaveTransaction(string savePointName)
        {
            if ((savePointName == null) || (savePointName.Length <= 0))
            {
                throw new ArgumentException("savePointName can't be null or empty", "savePointName");
            }
            if (this.IsTransactionInProgress == false)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }
            if (this.savePointNames.Contains(savePointName))
            {
                throw new ArgumentException("There is already a savepoint defined with the name '" + savePointName + "'", "savePointName");
            }

            MethodInfo saveMethod = this.activeTransaction.GetType().GetMethod("Save");
            if (saveMethod == null)
            {
                throw new NotSupportedException("The used .NET database provider doesn't support transaction saving, no Save(string) method present.");
            }
            saveMethod.Invoke(this.activeTransaction, new object[] { savePointName });
            this.savePointNames.Add(savePointName);
        }

        public void CommitTransaction()
        {
            if (!this.IsTransactionInProgress)
                return;
            //this.OnBeforeTransactionCommit();
            this.activeTransaction.Commit();
            this.IsTransactionInProgress = false;
            if (!this.KeepConnectionOpen)
            {
                this.CloseConnection();
                this.KeepConnectionOpen = false;
            }
            //this.OnAfterTransactionCommit();
        }

        public void RollbackTransaction()
        {
            if (!this.IsTransactionInProgress)
                return;
            //this.OnBeforeTransactionRollback();
            this.activeTransaction.Rollback();
            this.IsTransactionInProgress = false;
            if (!this.KeepConnectionOpen)
            {
                this.CloseConnection();
                this.KeepConnectionOpen = false;
            }
            //this.OnAfterTransactionRollback();
        }

        public void RollbackTransaction(string savePointName)
        {
            if ((savePointName == null) || (savePointName.Length <= 0))
            {
                throw new ArgumentException("savePointName can't be null or empty", "savePointName");
            }
            if (this.IsTransactionInProgress == false)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }
            if (!this.savePointNames.Contains(savePointName))
            {
                throw new ArgumentException("There is no savepoint defined with the name '" + savePointName + "'", "savePointName");
            }
            MethodInfo rollbackMethod = this.activeTransaction.GetType().GetMethod("Rollback", new Type[] { typeof(string) });
            if (rollbackMethod == null)
            {
                throw new NotSupportedException("The used .NET database provider doesn't support transaction rollback to a given point, no Rollback(string) method present.");
            }
            rollbackMethod.Invoke(this.activeTransaction, new object[] { savePointName });
            this.savePointNames.Remove(savePointName);
        }

        public bool IsTransactionInProgress
        {
            get;
            private set;
        }

        public IsolationLevel TransactionIsolationLevel { get; private set; }

        public string TransactionName { get; private set; }

        public abstract bool SupportsTransactionSavePoints { get; }

        public void AddLogListener(TextWriter textWriter)
        {
            if (!this.logListeners.Contains(textWriter))
                this.logListeners.Add(textWriter);
        }

        protected void WriteToLog(string message)
        {
            int i = 0;
            while (i < this.logListeners.Count)
            {
                var textWriter = this.logListeners[i];
                try
                {
                    textWriter.Write(message);
                }
                catch (IOException)
                {
                    // If the text writer failed, remove it
                    // and continue without incrementing the counter
                    // so that we still get the next text writer
                    // in the list.
                    this.logListeners.Remove(textWriter);
                    continue;
                }
                catch (ObjectDisposedException)
                {
                    // See above
                    this.logListeners.Remove(textWriter);
                    continue;
                }
                i++;
            }
        }

        protected void WriteLineToLog(string message)
        {
            this.WriteToLog(message + Environment.NewLine);
        }

        public IDatabaseSchema Schema { get; private set; }

        public abstract string QualifyTableName(ITable table);

        public string QualifyTableName(string tableName)
        {
            var table = this.Schema.FindTable(tableName);
            // Delegate creation to the specific provider.
            return this.QualifyTableName(table);
        }

        public abstract string QualifyColumnName(IColumn column);

        public abstract IDataParameter CreateParameter(int uniqueMarker, IColumn column, object value);

        public abstract IDataParameter CreateOutputParameter(int uniqueMarker, IColumn column);

        internal static string CreateParameterName(string fieldName)
        {
            return "@" + fieldName.Replace(" ", "_");
        }

        public abstract ITable DiscoverTableSchema(string tableName);

        /// <summary>
        /// Converts the OleDB type to a type compatible with the specified provider.
        /// </summary>
        internal static DbType ConvertOleDbTypeTo(int type)
        {
            // TODO implement this method
            return DbType.Object;
        }

        #region Data Provider Execution Methods

        /// <summary>
        /// Prepares the command for execution by setting up connection information to the command.
        /// </summary>
        /// <param name="command">The command.</param>
        internal void PrepareCommandForExecution(IDbCommand command)
        {
            if (command == null)
                throw new InvalidOperationException("No Command present. Nothing to execute.");
            if (this.activeConnection == null)
                this.CreateConnection();
            if (this.activeConnection == null)
                throw new InvalidOperationException("No Connection present. Cannot execute command.");
            command.Connection = this.activeConnection;
            command.CommandTimeout = this.CommandTimeout;
            if (this.IsTransactionInProgress)
            {
                command.Transaction = this.activeTransaction;
            }
        }

        /// <summary>
        /// Executes the passed in action query and, if not null, runs it inside the active transaction.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// execution result, which is the amount of rows affected (if applicable)
        /// </returns>
        public int ExecuteActionQuery(IDbCommand command)
        {
            try
            {
                this.PrepareCommandForExecution(command);
                this.OpenConnection();
                try
                {
                    int returnValue = command.ExecuteNonQuery();
                    return returnValue;
                }
                catch (Exception ex)
                {
                    throw new CommandExecutionException(
                        String.Format("An exception was caught during the execution of an action query: {0}. Check InnerException, QueryExecuted and Parameters of this exception to examine the cause of this exception.", ex.Message),
                            command.CommandText, command.Parameters, ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            finally
            {
                this.CloseConnectionIfPossible();

            }
        }

        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the active transaction. Executes
        /// the retrieval as a scalar query, returning the value.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns></returns>
        public object ExecuteScalarRetrievalQuery(IDbCommand command)
        {
            try
            {
                this.PrepareCommandForExecution(command);
                this.OpenConnection();
                try
                {
                    object returnValue = command.ExecuteScalar();
                    return returnValue;
                }
                catch (Exception ex)
                {
                    throw new CommandExecutionException(
                        String.Format("An exception was caught during the execution of a retrieval query: {0}. Check InnerException, QueryExecuted and Parameters of this exception to examine the cause of this exception.", ex.Message),
                            command.CommandText, command.Parameters, ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            finally
            {
                this.CloseConnectionIfPossible();
            }
        }

        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the active transaction. Used to read 1 row.
        /// It sets the connection object of the command object of query object passed in to the connection object of this class.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="fieldsToFill">The IEntityFields object to store the fetched data in</param>
        public void ExecuteSingleRowRetrievalQuery(IDbCommand command, IEntityFields fieldsToFill)
        {
            IDataReader dataReader = null;
            try
            {
                this.PrepareCommandForExecution(command);
                this.OpenConnection();
                try
                {
                    dataReader = command.ExecuteReader(CommandBehavior.SingleRow);
                    BaseDataProvider.FetchOneRow(dataReader, fieldsToFill);
                }
                catch (Exception ex)
                {
                    throw new CommandExecutionException(
                        String.Format("An exception was caught during the execution of a retrieval query: {0}. Check InnerException, QueryExecuted and Parameters of this exception to examine the cause of this exception.", ex.Message),
                            command.CommandText, command.Parameters, ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Dispose();
                this.CloseConnectionIfPossible();
            }
        }

        public IList<object> ExecuteMultiRowRetrievalQuery(IDbCommand command, IEntityCreator entityCreator, bool allowDuplicates)
        {
            IDataReader dataReader = null;
            try
            {
                this.PrepareCommandForExecution(command);

                bool wasConnectionClosed = (activeConnection.State != ConnectionState.Open);
                // execute the query
                try
                {
                    if (wasConnectionClosed)
                    {
                        this.OpenConnection();
                    }
                    dataReader = command.ExecuteReader(CommandBehavior.Default);
                    return this.FetchAllRows(dataReader, entityCreator, allowDuplicates);
                }
                catch (Exception ex)
                {
                    throw new CommandExecutionException(
                        String.Format("An exception was caught during the execution of a retrieval query: {0}. Check InnerException, QueryExecuted and Parameters of this exception to examine the cause of this exception.", ex.Message),
                            command.CommandText, command.Parameters, ex);
                }
                finally
                {
                    if (wasConnectionClosed)
                    {
                        this.CloseConnection();
                    }
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Dispose();
                this.CloseConnectionIfPossible();
            }
        }

        #region Row Helpers

        /// <summary>
        /// Fetches one row from the open data-reader and places that row into the passed in fields collection.
        /// </summary>
        /// <param name="dataReader">The open datareader used to fetch the data</param>
        /// <param name="fieldsToFill">The IEntityFields implementing object where the data should be stored.</param>
        private static void FetchOneRow(IDataReader dataReader, IEntityFields fieldsToFill)
        {
            if (dataReader == null)
            {
                return;
            }
            if (dataReader.IsClosed)
            {
                return;
            }
            if (fieldsToFill == null)
            {
                return;
            }
            // Read 1 row. First advance to first byte
            if (dataReader.Read())
            {
                object[] rowValues = new object[dataReader.FieldCount];
                try
                {
                    dataReader.GetValues(rowValues);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception caught in BaseSqlDataProvider.FetchOneRow: " + e.Message);
                    throw;
                }

                // Now read the actual row values into the fields collection
                BaseDataProvider.ReadRowIntoFields(rowValues, fieldsToFill);
            }
        }

        private IList<object> FetchAllRows(IDataReader dataReader, IEntityCreator entityCreator, bool allowDuplicates)
        {
            var fetchedInstances = new List<object>();
            if (dataReader == null || dataReader.IsClosed || entityCreator == null)
            {
                return fetchedInstances;
            }
            while (dataReader.Read())
            {
                object[] rowValues = new object[dataReader.FieldCount];
                try
                {
                    dataReader.GetValues(rowValues);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception caught in BaseSqlDataProvider.FetchAllRows: " + e.Message);
                    throw;
                }

                // Create the generic entity from the collection
                var objectInstance = entityCreator.Create();
                var entity = EntityProxyFactory.AsEntity(objectInstance) as IProxyEntity;
                // Create the empty fields object based on the schema
                if (!entity.Initialized)
                {
                    throw new InvalidOperationException("Entity was not initialized by the IEntityCreator");
                }

                // Now read the actual row values into the fields collection of the entity
                BaseDataProvider.ReadRowIntoFields(rowValues, entity.Fields);

                // set entity state
                entity.IsNew = false;

                if (allowDuplicates)
                {
                    fetchedInstances.Add(objectInstance);
                }
                else
                {
                    // Add the entity to the collection if it doesn't exist
                    // TODO PK logic
                    fetchedInstances.Add(objectInstance);
                }
            }
            return fetchedInstances;
        }

        /// <summary>
        /// Fetches the values passed in into the fieldsToFill.
        /// </summary>
        /// <param name="rowValues">The row values.</param>
        /// <param name="fieldsToFill">The IEntityFields implementing object where the data should be stored.</param>
        internal static void ReadRowIntoFields(object[] rowValues, IEntityFields fieldsToFill)
        {
            // The pairs should be sorted by the ordinal of the column in the rowValues array.
            for (int i = 0; i < rowValues.Length; i++)
            {
                EntityField fieldToSet = (EntityField)fieldsToFill[i];
                // Get the value
                object value = rowValues[i];
                // test for NULL
                bool isColumnValueDBNull = (value == System.DBNull.Value);
                // Set the value
                fieldToSet.IsNull = isColumnValueDBNull;
                fieldToSet.ForceSetCurrentValue(value);
            }
            // Set the fields state to fetched
            fieldsToFill.State = EntityState.Fetched;
            // Set the fields not as dirty since they are freshly fetched
            fieldsToFill.IsDirty = false;
        }

        #endregion

        #endregion
    }
}