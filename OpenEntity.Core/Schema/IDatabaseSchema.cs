using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Schema
{
    /// <summary>
    /// A representation of a database schema
    /// </summary>
    public interface IDatabaseSchema
    {
        /// <summary>
        /// A list of cached table information.
        /// </summary>
        /// <value>The tables.</value>
        IList<ITable> Tables { get; }
        /// <summary>
        /// Finds the schema for a particular table. Returns null if the table does not exist.
        /// </summary>
        /// <remarks>
        /// If the table is not in the schema cache, and the schema is discoverable, then it will
        /// attempt to use OleDb to aquire the schema using the default schema provided by the connection
        /// configuration.
        /// </remarks>
        ITable FindTable(string tableName);
        /// <summary>
        /// Determines whether the table with the specified name exists in the schema.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        /// 	<c>true</c> if the table with the specified name exists in the schema; otherwise, <c>false</c>.
        /// </returns>
        bool HasTable(string tableName);
        /// <summary>
        /// Gets the schema for the specified column in the specified table. Returns null if the table or column
        /// does not exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        IColumn FindColumn(string tableName, string columnName);
        /// <summary>
        /// Determines whether the column with the specified name exists in the schema.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>
        /// 	<c>true</c> if the column with the specified name exists in the schema; otherwise, <c>false</c>.
        /// </returns>
        bool HasColumn(string tableName, string columnName);        
        /// <summary>
        /// Sets the schema provider for this schema at runtime. There can be at most 1 schema provider per instance of a schema.
        /// </summary>
        /// <param name="schemaProvider">The schema provider.</param>
        void SetSchemaProvider(ISchemaProvider schemaProvider);
    }
}
