using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Joins
{
    public class Join
    {
        public Join(string originTable, string originColumn, string destinationTable, string destinationColumn, RelationshipType type)
        {
            this.OriginColumn = originColumn.ToUpper();
            this.OriginTable = originTable.ToUpper();
            this.DestinationColumn = destinationColumn.ToUpper();
            this.DestinationTable = destinationTable.ToUpper();
            this.RelationshipType = type;
        }

        private JoinSet destinationJoins;

        public RelationshipType RelationshipType { get; private set; }

        public string DestinationTable { get; set; }

        public string OriginTable { get; set; }

        public string DestinationColumn { get; set; }

        public string OriginColumn { get; set; }

        public JoinSet JoinSet
        {
            get
            {
                if (this.destinationJoins == null)
                    this.destinationJoins = new JoinSet(this.DestinationTable.ToUpper());
                return this.destinationJoins;
            }
        }
    }
}
