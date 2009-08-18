using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Schema
{
    /// <summary>
    /// Indicates a provider that is capable of retrieving schema information via discovery.
    /// </summary>
    public interface ISchemaProvider
    {
        /// <summary>
        /// Attempts to discover the schema for a particular table. Returns null if the table does not exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        ITable DiscoverTableSchema(string tableName);
    }
}
