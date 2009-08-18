using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;

namespace SchemaGrabber
{
    class Program
    {
        static string ConnectionString = @"server=.;Database=opcenter;trusted_connection=yes;provider=SQLOLEDB;";
        static string schema = @"dbo";
        static string filename = @"..\..\..\Enterprise\Database\PersistenceSchema.xml";

        static OleDbConnection connection;
        static PersistenceSchema persistenceSchema;

        static void Main(string[] args)
        {

            connection = new OleDbConnection();            
            connection.ConnectionString = ConnectionString;
            connection.Open();
            try
            {
                persistenceSchema = new PersistenceSchema();
                DataTable tables = connection.GetSchema("Tables");
                ParseTables(tables,schema);
                PersistenceHelper.WriteSchema(filename, persistenceSchema);
            }
            finally
            {
                connection.Close();
            }
            

        }

        static void ParseTables(DataTable tables, string schema)
        {
            foreach (DataRow row in tables.Rows)
            {
                if (row["TABLE_SCHEMA"].ToString() != schema)
                    continue;
                string tablename = row["TABLE_NAME"].ToString();
                schema = row["TABLE_SCHEMA"].ToString();
                string fullyQualifiedTableName = schema + ".[" + tablename + "]";


                Trace.WriteLine("+ Processing table: " + fullyQualifiedTableName);

                PersistenceTable table = new PersistenceTable() { Name = tablename, Schema = schema, EntityName = tablename };                
                var newFirst = Char.ToUpperInvariant(table.EntityName[0]);
                table.EntityName = table.EntityName.Remove(0, 1).Insert(0, newFirst.ToString());

                persistenceSchema.Tables.Add(table);

                using (OleDbCommand command = new OleDbCommand(fullyQualifiedTableName, connection))
                {
                    command.CommandType = CommandType.TableDirect;
                    try
                    {
                        using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
                        {
                            DataTable tableSchema = reader.GetSchemaTable();
                            ParseColumns(tableSchema, table);
                        }
                    }
                    catch (OleDbException exception)
                    {
                        Trace.WriteLine("! Exception:" + exception.Message);
                        throw;
                    }
                }
            }
        }

        static void ParseColumns(DataTable columns, PersistenceTable table)
        {
            Trace.Indent();

            foreach (DataRow row in columns.Rows)
            {
                string columnname = (string)row["ColumnName"];
                string dotnettype = ((Type)row["DataType"]).ToString();
                string dbtype = "System.Data.DbType."+GetDbType((OleDbType)row["ProviderType"]).ToString();
                string isidentity = row["IsAutoIncrement"].ToString().ToLower();
                string isprimarykey = row["IsKey"].ToString().ToLower();
                //schema.IsForeignKey = false; // Where do i get this info?
                string IsReadOnly = row["IsReadOnly"].ToString().ToLower();
                string IsNullable = row["AllowDBNull"].ToString().ToLower();
                string MaxLength = row["ColumnSize"].ToString();
                string Scale = row["NumericScale"].ToString();
                string Precision = row["NumericPrecision"].ToString();

                Trace.WriteLine("+ Processing column: " + columnname);

                // must be nullable, must be a value type, and must not be generic
                if (((Type)row["DataType"]).IsValueType && !((Type)row["DataType"]).IsGenericType && IsNullable.ToUpper().Contains("TRUE"))
                {
                    dotnettype = "Nullable<" + dotnettype + ">";
                }

                PersistenceField field = new PersistenceField()
                                        {
                                            Name = columnname,
                                            PropertyName = columnname,
                                            DotNetType = dotnettype,
                                            ForeignKey = "false",
                                            Identity = isidentity,
                                            Length = MaxLength,
                                            DbType = dbtype,
                                            Nullable = IsNullable,
                                            Precision = Precision,
                                            PrimaryKey = isprimarykey,
                                            Readonly = IsReadOnly,
                                            Scale = Scale
                                        };
                var newFirst = Char.ToUpperInvariant(field.PropertyName[0]);
                field.PropertyName = field.PropertyName.Remove(0, 1).Insert(0, newFirst.ToString());
                

                table.Fields.Add(field);
            }

            Trace.Unindent();
        }


        private static void DisplayData(System.Data.DataTable table)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
                }
                Console.WriteLine("============================");
            }
        }

        public static DbType GetDbType(OleDbType dbType)
        {
            switch (dbType)
            {
                case OleDbType.IDispatch:
                case OleDbType.Variant:
                case OleDbType.Empty:
                case OleDbType.IUnknown:
                case OleDbType.PropVariant:
                    return DbType.Object;
                case OleDbType.SmallInt:
                    return DbType.Int16;
                case OleDbType.Error:
                case OleDbType.Integer:
                    return DbType.Int32;
                case OleDbType.Single:
                    return DbType.Single;
                case OleDbType.Double:
                    return DbType.Double;
                case OleDbType.Currency:
                    return DbType.Currency;
                case OleDbType.Date:
                    return DbType.Date;
                case OleDbType.Filetime:
                case OleDbType.DBDate:
                case OleDbType.DBTimeStamp:
                    return DbType.DateTime;
                case OleDbType.BSTR:
                case OleDbType.VarChar:
                case OleDbType.Char:
                case OleDbType.WChar:
                case OleDbType.LongVarChar:
                case OleDbType.VarWChar:
                case OleDbType.LongVarWChar:
                    return DbType.String;
                case OleDbType.Boolean:
                    return DbType.Boolean;
                case OleDbType.Decimal:
                    return DbType.Decimal;
                case OleDbType.TinyInt:
                    return DbType.SByte;
                case OleDbType.UnsignedTinyInt:
                    return DbType.Byte;
                case OleDbType.UnsignedSmallInt:
                    return DbType.UInt16;
                case OleDbType.UnsignedInt:
                    return DbType.UInt32;
                case OleDbType.UnsignedBigInt:
                    return DbType.UInt64;
                case OleDbType.BigInt:
                    return DbType.Int64;
                case OleDbType.Guid:
                    return DbType.Guid;
                case OleDbType.Binary:
                case OleDbType.VarBinary:
                case OleDbType.LongVarBinary:
                    return DbType.Binary;
                case OleDbType.Numeric:
                case OleDbType.VarNumeric:
                    return DbType.VarNumeric;
                default:
                    return DbType.Object;
            }
        }

    }
}
