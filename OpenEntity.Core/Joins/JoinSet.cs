using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Joins
{
    public class JoinSet : IEnumerable<Join>
    {
        public JoinSet(string originTable)
        {
            this.OriginTable = originTable.ToUpper();
            this.relationships = new Dictionary<string, Join>();
        }

        private Dictionary<string, Join> relationships;

        public string OriginTable { get; set; }

        public Join AddOneToMany(string destinationTable, string originColumn, string destinationColumn)
        {
            if (this.relationships.ContainsKey(destinationTable.ToUpper()))
                throw new NotSupportedException("Destination table already exists as a join");
            Join relationship = new Join(this.OriginTable.ToUpper(), originColumn.ToUpper(), destinationTable.ToUpper(), destinationColumn.ToUpper(), RelationshipType.OneToMany);
            this.relationships.Add(destinationTable.ToUpper(), relationship);
            return this.Get(destinationTable.ToUpper());
        }

        public Join AddOneToOne(string destinationTable, string originColumn, string destinationColumn)
        {
            if (this.relationships.ContainsKey(destinationTable.ToUpper()))
                throw new NotSupportedException("Destination table already exists as a join");
            Join relationship = new Join(this.OriginTable.ToUpper(), originColumn.ToUpper(), destinationTable.ToUpper(), destinationColumn.ToUpper(), RelationshipType.OneToOne);
            this.relationships.Add(destinationTable.ToUpper(), relationship);
            return this.Get(destinationTable.ToUpper());
        }

        public Join Get(string destinationTable)
        {
            if (this.relationships.ContainsKey(destinationTable.ToUpper()))
                return this.relationships[destinationTable.ToUpper()];
            return null;
        }

        public int Count
        {
            get { return this.relationships.Count; }
        }

        #region IEnumerable<IJoin> Members

        public IEnumerator<Join> GetEnumerator()
        {
            return this.relationships.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.relationships.Values.GetEnumerator();
        }

        #endregion
    }
}
