using System.Data;

namespace Centro.OpenEntity.Schema
{
    /// <summary>
    /// General interface for table column schema information.
    /// </summary>
    public interface IColumn : IDatabaseObject
    {
        /// <summary>
        /// The type of the column. We only need the generic DbType for query generation.
        /// </summary>
        DbType DbDataType { get; }
        /// <summary>
        /// The .NET <see cref="System.Type"/> of the values of this column.
        /// </summary>
        System.Type DataType { get; }
        /// <summary>
        /// If set to true, the query engine will assume the column is an Identity column and will act accordingly (i.e.: it is an autoincrementing column in the database).
        /// </summary>
        bool IsIdentity { get; }
        /// <summary>
        /// If set to true, in the constructor, this column will end up in the PrimaryKey column list.
        /// </summary>
        bool IsPrimaryKey { get; }
        /// <summary>
        /// Will be true if this column can be set to NULL in the database, false otherwise. The column Validation logic in an entity will use this
        /// flag to check if the column indeed can be set to NULL or not. Set by constructor.
        /// </summary>
        bool IsNullable { get; }
        /// <summary>
        /// The column index, as determined by the data provider.
        /// </summary>
        int ColumnIndex { get; }
        /// <summary>
        /// If set to true, in the constructor, this column is part of a foreign key.
        /// </summary>
        bool IsForeignKey { get; }
        /// <summary>
        /// If set to true, in the constructor, no changes can be made to this column. 
        /// </summary>
        bool IsReadOnly { get; }
        /// <summary>
        /// The maximum length of the value of the column (string/binary data). Is ignored for column which hold non-string and non-binary values.
        /// Value initially set for this column is the length of the database column.
        /// </summary>
        int MaxLength { get; }
        /// <summary>
        /// The scale of the value for this column. 
        /// Value initially set for this column is the scale of the database column.
        /// </summary>
        short Scale { get; }
        /// <summary>
        /// The precision of the value for this column.
        /// Value initially set for this column is the precision of the database column.
        /// </summary>
        short Precision { get; }
        /// <summary>
        /// The schema for the table which this column belongs to.
        /// </summary>
        ITable Table { get; }
    }
}
