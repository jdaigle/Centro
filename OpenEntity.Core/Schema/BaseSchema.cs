using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using OpenEntity.Extensions;
using OpenEntity.Helpers;

namespace OpenEntity.Schema
{
    /// <summary>
    /// A base abstract implementation of an ISchema.
    /// </summary>
    internal abstract class BaseSchema : IDatabaseSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSchema"/> class.
        /// </summary>
        protected BaseSchema()
        {
            this.Tables = new List<ITable>();
            this.InitSchema();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSchema"/> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        protected BaseSchema(BaseSchema schema)
        {
            ContractUtils.Requires(schema != null, "schema");
            this.Tables = schema.Tables.ToList();
        }

        public IList<ITable> Tables { get; private set; }

        public ITable FindTable(string tableName)
        {
            ITable table = this.Tables.FirstOrDefault(t => t.Name.Matches(tableName));
            if (table != null)
                return table;
            else if (this.TryDiscoverTableSchema(tableName))
            {
                return this.Tables.FirstOrDefault(t => t.Name.Matches(tableName));
            }
            return null;
        }

        public bool HasTable(string tableName)
        {
            return this.FindTable(tableName) != null;
        }

        public IColumn FindColumn(string tableName, string columnName)
        {
            var table = this.FindTable(tableName);
            if (table != null)
                return table.Columns.FirstOrDefault(c => c.Name.Matches(columnName));
            return null;
        }

        public bool HasColumn(string tableName, string columnName)
        {
            return this.FindColumn(tableName, columnName) != null;
        }

        /// <summary>
        /// Gets the schema provider.
        /// </summary>
        protected ISchemaProvider SchemaProvider { get; private set; }

        public void SetSchemaProvider(ISchemaProvider schemaProvider)
        {
            this.SchemaProvider = schemaProvider;
        }

        /// <summary>
        /// Initializes the schema at runtime.
        /// </summary>
        protected abstract void InitSchema();

        /// <summary>
        /// Adds a table to the schema if it doesn't already exist.
        /// </summary>
        /// <param name="table">The table.</param>
        protected void AddTable(ITable table)
        {
            if (!this.Tables.Any(t => t.Name.Matches(table.Name)))
                this.Tables.Add(table);
        }

        /// <summary>
        /// A helper method to try to obtain the schema for a particular table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        private bool TryDiscoverTableSchema(string tableName)
        {
            if (this.SchemaProvider == null)
                return false;
            ITable table = this.SchemaProvider.DiscoverTableSchema(tableName);
            if (table == null)
                return false;
            this.AddTable(table);
            return true;
        }
    }
}
