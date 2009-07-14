using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Extensions;


namespace OpenEntity.Schema
{
    public class DatabaseTable : ITable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseTable"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="schemaName">Name of the schema.</param>
        protected internal DatabaseTable(string name, string schemaName)
        {
            this.Name = name;
            this.SchemaName = schemaName;
            this.columns = new List<IColumn>();
        }

        private IList<IColumn> columns;

        /// <summary>
        /// Adds a column if it doesn't exist.
        /// </summary>
        /// <param name="column">The column.</param>
        internal void AddColumn(IColumn column)
        {
            if (this.columns.Where(c => c.Name.Matches(column.Name)).Count() == 0)
            {
                (column as DatabaseColumn).Table = this;
                this.columns.Add(column);
            }
        }

        #region IDatabaseObjectSchema Members

        public string Name
        {
            get;
            private set;
        }

        public string SchemaName
        {
            get;
            private set;
        }

        #endregion

        public IList<IColumn> Columns
        {
            get { return this.columns; }
            internal set
            {
                this.columns = value;
            }
        }

        public bool HasPrimaryKey
        {
            get { return this.PrimaryKeys.Count() > 0; }
        }

        public IList<IColumn> PrimaryKeys
        {
            get
            {
                return this.Columns.Where(c => c.IsPrimaryKey).ToList();
            }
        }

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
