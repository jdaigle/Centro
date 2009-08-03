using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using OpenEntity.Schema;

namespace OpenEntity.DataProviders
{
    /// <summary>
    /// Implementation of BaseDataProvider specified to Microsoft SQL Server (version agnostic).
    /// </summary>
    public class SqlServerDataProvider : BaseDataProvider
    {
        private string defaultSchemaName;
        private string oleDbConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        internal SqlServerDataProvider(string connectionString)
            : base(connectionString)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.ConnectionString);
            var oleDbConnectionStringBuilder = new OleDbConnectionStringBuilder();
            oleDbConnectionStringBuilder.DataSource = sqlConnectionStringBuilder.DataSource;
            oleDbConnectionStringBuilder.Provider = "SQLOLEDB";
            oleDbConnectionStringBuilder["database"] = sqlConnectionStringBuilder.InitialCatalog;
            oleDbConnectionStringBuilder["trusted_connection"] = sqlConnectionStringBuilder.IntegratedSecurity ? "yes" : "no";
            this.oleDbConnectionString = oleDbConnectionStringBuilder.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultSchemaName">The name of the default schema.</param>
        internal SqlServerDataProvider(string connectionString, string defaultSchemaName)
            : this(connectionString)
        {
            this.defaultSchemaName = defaultSchemaName;
        }

        /// <summary>
        /// Gets the the name of the default schema for this data provider.
        /// </summary>
        protected override string GetDefaultSchemaName()
        {
            if (string.IsNullOrEmpty(this.defaultSchemaName))
                return "dbo";
            else
                return this.defaultSchemaName;
        }

        /// <summary>
        /// Creates a new physical connection object.
        /// </summary>
        /// <param name="connectionParameters">The connection parameters.</param>
        /// <returns>
        /// IDbConnection implementing connection object.
        /// </returns>
        protected override IDbConnection CreateNewConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Creates a new physical transaction object over the created connection. The connection is assumed to be open.
        /// </summary>
        /// <returns>a physical transaction object, like an instance of SqlTransaction.</returns>
        protected override IDbTransaction CreateNewTransaction()
        {
            return this.GetActiveConnection().BeginTransaction(this.TransactionIsolationLevel);
        }

        /// <summary>
        /// Creates a new .NET DataAdapter for the database system this DataAccessAdapter object is targeting. 
        /// </summary>
        /// <returns>New .NET DataAdapter object</returns>
        protected override DbDataAdapter CreateNewDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        /// Creates a new client specific IDbCommand object.
        /// </summary>
        public override IDbCommand CreateDbCommand()
        {
            return new SqlCommand();
        }

        public override string QualifyTableName(ITable table)
        {
            // schema.table
            return string.Format("[{0}].[{1}]", table.SchemaName, table.Name);
        }

        public override string QualifyColumnName(IColumn column)
        {
            // schema.table.column
            return string.Format("[{0}].[{1}].[{2}]", column.Table.SchemaName, column.Table.Name, column.Name);
        }

        public override bool SupportsTransactionSavePoints
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Creates an output parameter based on information in this schema.
        /// </summary>
        /// <param name="uniqueMarker">The unique marker use in the parameter name.</param>
        /// <param name="column">The column schema used to the generate the parameter.</param>
        /// <returns></returns>
        public override IDataParameter CreateOutputParameter(int uniqueMarker, IColumn column)
        {
            SqlDbType dbTypeToUse = SqlDbType.VarChar;
            switch (column.DataType.UnderlyingSystemType.FullName)
            {
                case "System.String":
                    dbTypeToUse = SqlDbType.VarChar;
                    break;
                case "System.Byte":
                    dbTypeToUse = SqlDbType.TinyInt;
                    break;
                case "System.Int32":
                    dbTypeToUse = SqlDbType.Int;
                    break;
                case "System.Int16":
                    dbTypeToUse = SqlDbType.SmallInt;
                    break;
                case "System.Int64":
                    dbTypeToUse = SqlDbType.BigInt;
                    break;
                case "System.DateTime":
                    dbTypeToUse = SqlDbType.DateTime;
                    break;
                case "System.Decimal":
                    dbTypeToUse = SqlDbType.Decimal;
                    break;
                case "System.Double":
                    dbTypeToUse = SqlDbType.Float;
                    break;
                case "System.Single":
                    dbTypeToUse = SqlDbType.Real;
                    break;
                case "System.Boolean":
                    dbTypeToUse = SqlDbType.Bit;
                    break;
                case "System.Byte[]":
                    dbTypeToUse = SqlDbType.VarBinary;
                    break;
                case "System.Guid":
                    dbTypeToUse = SqlDbType.UniqueIdentifier;
                    break;
                default:
                    dbTypeToUse = SqlDbType.VarChar;
                    break;
            }

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = BaseDataProvider.CreateParameterName(column.Name) + uniqueMarker.ToString();
            sqlParameter.DbType = new SqlParameter("@Dummy", dbTypeToUse).DbType;
            sqlParameter.Direction = ParameterDirection.Output;
            sqlParameter.SourceColumn = this.QualifyColumnName(column);
            return sqlParameter;
        }

        /// <summary>
        /// Creates a parameter based on information in this schema.
        /// </summary>
        /// <param name="uniqueMarker">The unique marker use in the parameter name.</param>
        /// <param name="column">The column schema used to the generate the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <returns></returns>
        public override IDataParameter CreateParameter(int uniqueMarker, IColumn column, object value)
        {
            SqlDbType dbTypeToUse = SqlDbType.VarChar;
            if (value != null)
            {
                switch (value.GetType().UnderlyingSystemType.FullName)
                {
                    case "System.String":
                        if (((string)value).Length < 4000)
                        {
                            dbTypeToUse = SqlDbType.NVarChar;
                        }
                        else
                        {
                            if (((string)value).Length < 8000)
                            {
                                dbTypeToUse = SqlDbType.VarChar;
                            }
                            else
                            {
                                dbTypeToUse = SqlDbType.Text;
                            }
                        }
                        break;
                    case "System.Byte":
                        dbTypeToUse = SqlDbType.TinyInt;
                        break;
                    case "System.Int32":
                        dbTypeToUse = SqlDbType.Int;
                        break;
                    case "System.Int16":
                        dbTypeToUse = SqlDbType.SmallInt;
                        break;
                    case "System.Int64":
                        dbTypeToUse = SqlDbType.BigInt;
                        break;
                    case "System.DateTime":
                        dbTypeToUse = SqlDbType.DateTime;
                        break;
                    case "System.Decimal":
                        dbTypeToUse = SqlDbType.Decimal;
                        break;
                    case "System.Double":
                        dbTypeToUse = SqlDbType.Float;
                        break;
                    case "System.Single":
                        dbTypeToUse = SqlDbType.Real;
                        break;
                    case "System.Boolean":
                        dbTypeToUse = SqlDbType.Bit;
                        break;
                    case "System.Byte[]":
                        byte[] valueAsArray = (byte[])value;
                        if (valueAsArray.Length < 8000)
                        {
                            dbTypeToUse = SqlDbType.VarBinary;
                        }
                        else
                        {
                            dbTypeToUse = SqlDbType.Image;
                        }
                        break;
                    case "System.Guid":
                        dbTypeToUse = SqlDbType.UniqueIdentifier;
                        break;
                    default:
                        dbTypeToUse = SqlDbType.VarChar;
                        break;
                }
            }

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = BaseDataProvider.CreateParameterName(column.Name) + uniqueMarker.ToString();
            sqlParameter.DbType = new SqlParameter("@Dummy", dbTypeToUse).DbType;
            sqlParameter.IsNullable = column.IsNullable;
            sqlParameter.SourceColumn = this.QualifyColumnName(column);
            if (value == null)
            {
                sqlParameter.Value = System.DBNull.Value;
            }
            else
            {
                sqlParameter.Value = value;
            }
            return sqlParameter;
        }

        /// <summary>
        /// Attemps to discover the ITable for a particular table. Returns null if the table does not exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public override ITable DiscoverTableSchema(string tableName)
        {
            using (var connection = new OleDbConnection(this.oleDbConnectionString))
            {
                connection.Open();
                try
                {
                    // Get the default schema from the configuration
                    string schemaName = this.GetDefaultSchemaName();
                    string fullyQualifiedTableName = schemaName + ".[" + tableName+"]";
                    using (OleDbCommand command = new OleDbCommand(fullyQualifiedTableName, connection))
                    {
                        command.CommandType = CommandType.TableDirect;
                        try
                        {
                            using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
                            {
                                DataTable schemaTable = reader.GetSchemaTable();
                                if (schemaTable == null)
                                    return null;
                                List<IColumn> columns = new List<IColumn>();
                                foreach (DataRow row in schemaTable.Rows)
                                {
                                    if (row.Table.Columns.Contains("IsHidden") && (bool)row["IsHidden"])
                                    {
                                        // Don't process "hidden" columns. Some views have duplicate columns which are hidden.
                                        continue;
                                    }
                                    columns.Add(this.ParseColumnSchema(row));
                                }
                                // Only if we obtained more than 1 column.
                                if (columns.Count > 1)
                                {
                                    // The table schema will be the default schema we pulled from
                                    DatabaseTable table = new DatabaseTable(tableName, schemaName);
                                    foreach (IColumn column in columns)
                                        table.AddColumn(column);
                                    return table;
                                }
                            }
                        }
                        catch (OleDbException exception)
                        {
                            // Table not found
                            if (exception.ErrorCode == -2147217865)
                                return null;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Parses OleDb schema column information into a IColumn
        /// </summary>
        private IColumn ParseColumnSchema(DataRow row)
        {
            var schema = new DatabaseColumn();
            schema.Name = (string)row["ColumnName"];
            //schema.SchemaName = (string)row["BaseSchemaName"]; // Schema is determined by the tablename
            schema.DataType = (Type)row["DataType"];
            schema.DbDataType = BaseDataProvider.ConvertOleDbTypeTo((int)row["ProviderType"]); // Need to convert this to the correct provider data type
            schema.IsIdentity = (bool)row["IsAutoIncrement"];
            schema.IsPrimaryKey = (bool)row["IsKey"];
            schema.IsForeignKey = false; // Where do i get this info?
            schema.IsReadOnly = (bool)row["IsReadOnly"];
            schema.IsNullable = (bool)row["AllowDBNull"];
            schema.ColumnIndex = (int)row["ColumnOrdinal"];
            schema.MaxLength = (int)row["ColumnSize"];
            schema.Scale = (short)row["NumericScale"];
            schema.Precision = (short)row["NumericPrecision"];
            return schema;
        }
    }
}
