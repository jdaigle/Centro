using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using OpenEntity.Schema;
using OpenEntity.Entities;

namespace OpenEntity.DataProviders
{
    /// <summary>
    /// A data provider provides access to a relational database persistent storage.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Opens the active connection object. If the connection is already open, nothing is done.
        /// If no connection object is present, a new one is created
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// Closes the active connection. If no connection is available or the connection is closed, nothing is done.
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// Gets / sets the timeout value to use with the command object(s) created by the repository.
        /// Set this prior to calling a method which executes database logic.
        /// </summary>
        int CommandTimeout { get; set; }
        /// <summary>
        /// Gets the data connection string for this data provider.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Gets / sets KeepConnectionOpen, a flag used to keep open connections after a database action has finished.
        /// </summary>
        bool KeepConnectionOpen { get; set; }
        /// <summary>
        /// Starts a new transaction. All database activity after this call will be ran in this transaction and all objects will participate
        /// in this transaction until its committed or rolled back. 
        /// If there is a transaction in progress, an exception is thrown.
        /// Will create and open a new connection if a transaction is not open and/or available.
        /// </summary>
        /// <param name="isolationLevelToUse">The isolation level to use for this transaction</param>
        /// <param name="name">The name for this transaction</param>
        void StartTransaction(IsolationLevel isolationLevelToUse, string name);
        /// <summary>
        /// Starts a new transaction using the default isolationlevel. All database activity after this call will be ran in this transaction and all objects will participate
        /// in this transaction until its committed or rolled back. 
        /// If there is a transaction in progress, an exception is thrown.
        /// Will create and open a new connection if a transaction is not open and/or available.
        /// </summary>
        /// <param name="name">The name for this transaction</param>
        void StartTransaction(string name);
        /// <summary>
        /// Creates a savepoint with the name savePointName in the current transaction. You can roll back to this savepoint using
        /// <see cref="Rollback(string)"/>.
        /// </summary>
        /// <param name="savePointName">name of savepoint. Must be unique in an active transaction</param>
        /// <exception cref="InvalidOperationException">If no transaction is in progress.</exception>
        /// <exception cref="ArgumentException">if savePointName is empty or null</exception>
        /// <exception cref="NotSupportedException">If the database provider doesn't support transaction saving.</exception>
        void SaveTransaction(string savePointName);
        /// <summary>
        /// Commits the transaction in action. It will end all database activity, since commiting a transaction is finalizing it. After
        /// calling Commit or Rollback, the ITransaction implementing class will reset itself.
        /// </summary>
        /// <remarks>Will close the active connection unless KeepConnectionOpen has been set to true</remarks>
        void CommitTransaction();
        /// <summary>
        /// Rolls back the transaction in action. It will end all database activity, since commiting a transaction is finalizing it. After
        /// calling Commit or Rollback, the ITransaction implementing class will reset itself. 
        /// </summary>
        /// <remarks>Will close the active connection unless KeepConnectionOpen has been set to true</remarks>
        void RollbackTransaction();
        /// <summary>
        /// Rolls back the transaction in action to the savepoint with the name savepointName. No internal objects are being reset when this method is called,
        /// so call this Rollback overload only to roll back to a savepoint. To roll back a complete transaction, call Rollback() without specifying a savepoint
        /// name. Create a savepoint by calling SaveTransaction(savePointName)
        /// </summary>
        /// <exception cref="InvalidOperationException">If no transaction is in progress.</exception>
        /// <exception cref="ArgumentException">if savePointName is empty or null</exception>
        /// <exception cref="NotSupportedException">if the database provider doesn't support transaction rolling back</exception>
        /// <param name="savePointName">name of the savepoint to roll back to.</param>
        void RollbackTransaction(string savePointName);
        /// <summary>
        /// Gets IsTransactionInProgress. True when there is a transaction in progress.
        /// </summary>
        bool IsTransactionInProgress { get; }
        /// <summary>
        /// Gets the isolation level a transaction should use.
        /// </summary>
        IsolationLevel TransactionIsolationLevel { get; }
        /// <summary>
        /// Gets the name of the transaction.
        /// </summary>
        string TransactionName { get; }
        /// <summary>
        /// Gets a value indicating whether this data provider supports transaction save points.
        /// </summary>
        bool SupportsTransactionSavePoints { get; }
        /// <summary>
        /// Adds a TextWriter which will be written to anytime a SQL statement is queries or executed through the data provider.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        void AddLogListener(TextWriter textWriter);
        /// <summary>
        /// Holds information on tables, views, stored procedures, etc. for the database this data provider is connected to.
        /// </summary>
        IDatabaseSchema Schema { get; }
        /// <summary>
        /// Qualifies the name of the table.
        /// </summary>
        /// <param name="table">The table.</param>
        string QualifyTableName(ITable table);
        /// <summary>
        /// Qualifies the name of the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        string QualifyTableName(string tableName);
        /// <summary>
        /// Qualifies the name of the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        string QualifyColumnName(IColumn column);
        /// <summary>
        /// Creates a parameter based on information in this schema.
        /// </summary>
        /// <param name="uniqueMarker">The unique marker use in the parameter name.</param>
        /// <param name="column">The column schema used to the generate the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <returns></returns>
        IDataParameter CreateParameter(int uniqueMarker, IColumn column, object value);
        /// <summary>
        /// Creates an output parameter based on information in this schema.
        /// </summary>
        /// <param name="uniqueMarker">The unique marker use in the parameter name.</param>
        /// <param name="column">The column schema used to the generate the parameter.</param>
        /// <returns></returns>
        IDataParameter CreateOutputParameter(int uniqueMarker, IColumn column);
        /// <summary>
        /// Creates a new client specific IDbCommand object.
        /// </summary>
        IDbCommand CreateDbCommand();
        /// <summary>
        /// Executes the passed in action query and, if not null, runs it inside the active transaction.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// execution result, which is the amount of rows affected (if applicable)
        /// </returns>
        int ExecuteActionQuery(IDbCommand command);
        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the active transaction. Executes
        /// the retrieval as a scalar query, returning the value.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns></returns>
        object ExecuteScalarRetrievalQuery(IDbCommand command);
        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the active transaction. Used to read 1 row.
        /// It sets the connection object of the command object of query object passed in to the connection object of this class.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="fieldsToFill">The IEntityFields object to store the fetched data in</param>
        void ExecuteSingleRowRetrievalQuery(IDbCommand command, IEntityFields fieldsToFill);
        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the active transaction. Used to read 1 or more rows.
        /// It sets the connection object of the command object of query object passed in to the connection object of this class.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="entityCreator">The entity creator.</param>
        /// <param name="allowDuplicates">Flag to signal if duplicates in the datastream should be loaded into the collection (true) or not (false)</param>
        IList<IEntity> ExecuteMultiRowRetrievalQuery(IDbCommand command, IEntityCreator entityCreator, bool allowDuplicates);
    }
}
