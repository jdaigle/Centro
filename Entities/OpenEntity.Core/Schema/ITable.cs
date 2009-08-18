using System.Collections.Generic;

namespace OpenEntity.Schema
{
    /// <summary>
    /// General interface for table schema information.
    /// </summary>
    public interface ITable : IDatabaseObject
    {
        /// <summary>
        /// A list of columns in this table.
        /// </summary>
        IList<IColumn> Columns { get; }
        /// <summary>
        /// Gets a value indicating whether this table contains a primary key.
        /// </summary>
        bool HasPrimaryKey { get; }
        /// <summary>
        /// A list of columns representing the primary key.
        /// </summary>
        IList<IColumn> PrimaryKeys { get; }
    }
}
