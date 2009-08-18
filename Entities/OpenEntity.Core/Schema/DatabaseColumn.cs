using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Centro.OpenEntity.Schema
{

    /// <summary>
    /// General container class for static column information which is readonly at runtime and which is equal for all instances of a 
    /// given field. This information is shared among all instances of an entity, and therefore saves a lot of memory at runtime.
    /// </summary>
    internal class DatabaseColumn : IColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseColumn"/> class.
        /// </summary>
        protected internal DatabaseColumn()
        {
        }

        public string Name { get; internal set; }

        public Type DataType { get; internal set; }

        public bool IsPrimaryKey { get; internal set; }

        public bool IsNullable { get; internal set; }

        public int ColumnIndex { get; internal set; }

        public bool IsForeignKey { get; internal set; }

        public bool IsReadOnly { get; internal set; }

        public int MaxLength { get; internal set; }

        public short Scale { get; internal set; }

        public short Precision { get; internal set; }

        public bool IsIdentity { get; internal set; }

        public DbType DbDataType { get; internal set; }

        public string SchemaName
        {
            get
            {
                if (this.Table != null)
                    return this.Table.SchemaName;
                return string.Empty;
            }
        }

        public ITable Table { get; internal set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.SchemaName + "." + this.Name;
        }
    }
}
